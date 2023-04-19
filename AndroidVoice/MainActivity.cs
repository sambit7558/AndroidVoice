using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidVoiceLibrary;
using System;
using AndroidX.AppCompat.Widget;
using Android.Content;

namespace AndroidVoice
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private ImageView recButton;
        private VoiceService voiceService;
        private string languageCode;
        private int SPEECH_REQUEST_CODE = 0;
        private TextView TView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            languageCode = null;
            TView = FindViewById<TextView>(Resource.Id.textView1);

            //-------------------||If we want to set the text in search view||-------------------
            //searchView = FindViewById<SearchView>(Resource.Id.searchView1);
            //searchView.SetOnQueryTextListener(this);

            //-------------------||If you want to support multiple language||------------------

            //// Get the spinner widget
            //Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerLanguage);

            //// Set up the spinner adapter
            //ArrayAdapter adapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.languages, Android.Resource.Layout.SimpleSpinnerItem);
            //adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner.Adapter = adapter1;

            //string defaultLanguageCode = Java.Util.Locale.Default.Language;



            //// Set the initial language to Default
            //int initialLanguageIndex = 0;
            //spinner.SetSelection(initialLanguageIndex);

            //// Set up the spinner item selected listener
            //spinner.ItemSelected += (sender, e) =>
            //{
            //    languageCode = null;

            //    switch (e.Position)
            //    {
            //        case 0:
            //            // Default Android language
            //            languageCode = defaultLanguageCode;
            //            break;
            //        case 1:
            //            // Chinese(PRC) language
            //            languageCode = "zh-CN";
            //            break;
            //        case 2:
            //             //Japanese language
            //            languageCode = "ja-JP";
            //            break;


            //---------Give as many language code as you need
    //------------------------------------------------------------------------------------------------
            //------also add the languages in the strings.xml of your app like given below
            //<string-array name="languages">

            //<item>Default</item>
            //<item>Chinese(PRC)</item>
            //<item>Japanese</item>

            //as many language as you want to add

            //</string-array>
    //---------------------------------------------------------------------------------------------
            //        default:
            //            // Default Android language
            //            languageCode = defaultLanguageCode;
            //            break;

            //    }

            //    // Update SPEECH_REQUEST_CODE variable to keep track of the current language
            //    SPEECH_REQUEST_CODE = e.Position;

            //};

            string defaultLanguageCode = Java.Util.Locale.Default.Language;
            languageCode = defaultLanguageCode;

            recButton = FindViewById<ImageView>(Resource.Id.btnRecord);

            voiceService = new VoiceService();

            recButton.Click += delegate
            {
                voiceService.StartVoiceRecognition(this, OnVoiceRecognitionResult, languageCode, SPEECH_REQUEST_CODE);
            };
        }

        //-------------------------||If you want to set the text in the search view||---------------------
        //private void OnVoiceRecognitionResult(string result)
        //{
        //    // Do something with the voice recognition result
        //    searchView.SetQuery(result, true);
        //}

    private void OnVoiceRecognitionResult(string result)
        {
            // Do something with the voice recognition result
            TView.SetText(result, TextView.BufferType.Normal);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            // Call the OnResults method of the voiceService instance
            voiceService.OnResults(requestCode, resultCode, data, SPEECH_REQUEST_CODE);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}