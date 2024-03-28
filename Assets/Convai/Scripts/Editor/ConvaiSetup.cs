#if UNITY_EDITOR

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Convai.Scripts.Utils;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Convai.Scripts.Editor
{
    public class ConvaiSetup : EditorWindow
    {
        private const string API_KEY_PATH = "Assets/Resources/ConvaiAPIKey.asset";
        private const string API_URL = "https://api.convai.com/user/referral-source-status";

        private void OnEnable()
        {
            if (!File.Exists(API_KEY_PATH)) SetupConvaiAPIKey();
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            ScrollView page2 = new();

            // Add Logo
            root.Add(CreateImage("Assets/Convai/Images/color.png", 100));

            // Add Elements to Page
            page2.Add(CreateLabel("Enter your API Key:", 16));
            TextField apiKeyTextField = new("", -1, false, true, '*');
            page2.Add(apiKeyTextField);

            page2.Add(CreateButton("Begin!", 16, FontStyle.Bold, () =>
            {
                HandleBeginButtonAction(apiKeyTextField.text).ContinueWith(t =>
                {
                    if (t.Exception != null)
                        // Optionally handle exceptions
                        Debug.LogError($"Error while processing API Key: {t.Exception.InnerException?.Message}");
                });
            }));


            page2.Add(CreateButton("How do I find my API key?", 12, FontStyle.Normal,
                () => { Application.OpenURL("https://docs.convai.com/api-docs/plugins/unity-plugin"); }));

            // Set Margins for Page
            SetMargins(page2, 20);

            root.Add(page2);
        }

        private async Task HandleBeginButtonAction(string apiKey)
        {
            bool validKey = await BeginButtonTask(apiKey);
            if (validKey)
                Close();
            // if the key is not valid, the window remains open
        }


        //Helper methods for creating UI elements and styles:
        private Image CreateImage(string path, float height)
        {
            Image image = new()
            {
                image = AssetDatabase.LoadAssetAtPath<Texture>(path),
                style =
                {
                    height = height
                }
            };
            SetPadding(image, 10);
            return image;
        }

        private Label CreateLabel(string text, int fontSize)
        {
            return new Label(text) { style = { fontSize = fontSize } };
        }

        private Button CreateButton(string text, int fontSize, FontStyle fontStyle, Action onClickAction)
        {
            Button button = new(onClickAction)
            {
                text = text,
                style =
                {
                    fontSize = fontSize,
                    unityFontStyleAndWeight = fontStyle,
                    alignSelf = Align.Center
                }
            };
            SetPadding(button, 10);
            return button;
        }

        private void SetMargins(VisualElement element, float margin)
        {
            element.style.marginBottom = margin;
            element.style.marginLeft = margin;
            element.style.marginRight = margin;
            element.style.marginTop = margin;
        }

        private void SetPadding(VisualElement element, float padding)
        {
            element.style.paddingBottom = padding;
            element.style.paddingLeft = padding;
            element.style.paddingRight = padding;
            element.style.paddingTop = padding;
        }


        [MenuItem("Convai/Convai Setup", false, 1)]
        public static void SetupConvaiAPIKey()
        {
            GetWindow<ConvaiSetup>().titleContent = new GUIContent("Convai Setup");
        }

        [MenuItem("Convai/Documentation", false, 5)]
        public static void OpenDocumentation()
        {
            Application.OpenURL("https://docs.convai.com/plugins-and-integrations/unity-plugin");
        }

        private async Task<string> CheckReferralStatus(string url, string apiKey)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("CONVAI-API-KEY", apiKey);

            try
            {
                // Send the request
                await using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    byte[] jsonBytes = Encoding.UTF8.GetBytes("{}");
                    await requestStream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
                }

                // Read the response
                using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                await using Stream streamResponse = response.GetResponseStream();
                if (streamResponse != null)
                {
                    using StreamReader reader = new(streamResponse);

                    string responseContent = await reader.ReadToEndAsync();
                    ReferralSourceStatus referralStatus =
                        JsonConvert.DeserializeObject<ReferralSourceStatus>(responseContent);

                    return referralStatus.ReferralSourceStatusProperty;
                }
            }
            catch (WebException e)
            {
                Debug.LogError($"{e.Message}\nPlease check if API Key is correct.");
                return string.Empty;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return string.Empty;
            }

            return string.Empty; // Added this return for the case where streamResponse is null
        }


        private async Task<bool> BeginButtonTask(string apiKey)
        {
            ConvaiAPIKeySetup aPIKeySetup = CreateInstance<ConvaiAPIKeySetup>();

            aPIKeySetup.APIKey = apiKey;

            if (!string.IsNullOrEmpty(apiKey))
            {
                string referralStatus =
                    await CheckReferralStatus(API_URL, apiKey);

                if (!string.IsNullOrEmpty(referralStatus))
                {
                    CreateOrUpdateAPIKeyAsset(aPIKeySetup);

                    if (referralStatus == "undefined")
                    {
                        EditorUtility.DisplayDialog("Warning", "API Key loaded successfully with undefined status!",
                            "OK");
                        return true;
                    }

                    EditorUtility.DisplayDialog("Success", "API Key loaded successfully!", "OK");
                    return true;
                }

                EditorUtility.DisplayDialog("Error", "Please enter a valid API Key.", "OK");
                return false;
            }

            EditorUtility.DisplayDialog("Error", "Please enter a valid API Key.", "OK");
            return false;
        }


        private void CreateOrUpdateAPIKeyAsset(ConvaiAPIKeySetup aPIKeySetup)
        {
            string assetPath = "Assets/Resources/ConvaiAPIKey.asset";

            if (!File.Exists(assetPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");

                AssetDatabase.CreateAsset(aPIKeySetup, assetPath);
            }
            else
            {
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.CreateAsset(aPIKeySetup, assetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        public class UpdateSource
        {
            public UpdateSource(string referralSource)
            {
                ReferralSource = referralSource;
            }

            [JsonProperty("referral_source")] public string ReferralSource { get; set; }
        }

        public class ReferralSourceStatus
        {
            [JsonProperty("referral_source_status")]
            public string
                ReferralSourceStatusProperty { get; set; } // Renamed it to avoid confusion with the class name

            [JsonProperty("status")] public string Status { get; set; }
        }
    }
}
#endif