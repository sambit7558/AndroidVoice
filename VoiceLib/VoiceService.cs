using Android;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidVoiceLibrary
{
    public class VoiceService : Java.Lang.Object, IRecognitionListener
    { 
        private SpeechRecognizer speechRecognizer;
        private Action<string> callback;
        public void StartVoiceRecognition(Activity activity, Action<string> callback, string languageCode, int SPEECH_REQUEST_CODE)
        {
            this.callback = callback;

            // Initialize speech recognizer
            speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(activity);
            speechRecognizer.SetRecognitionListener(this);


            // Check for microphone
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                var alert = new Android.App.AlertDialog.Builder(activity);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    return;
                });
                alert.Show();
                return;
            }

            // Create speech recognition intent
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "speak now!");
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 5000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 5000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, languageCode);

            // Start speech recognition activity
            activity.StartActivityForResult(voiceIntent, SPEECH_REQUEST_CODE);
        }

        public void OnResults(int requestCode, Result resultCode, Intent data, int SPEECH_REQUEST_CODE)
        {


            if (requestCode == SPEECH_REQUEST_CODE && resultCode == Result.Ok)
            {
                // Get speech recognition results
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                if (matches != null && matches.Count > 0)
                {
                    // Display first result in text view
                    var text = matches[0];
                    callback.Invoke(text);
                }
            }
        }

        public void OnBeginningOfSpeech()
        {
            // Not used
        }

        public void OnBufferReceived(byte[] buffer)
        {
            // Not used
        }

        public void OnEndOfSpeech()
        {
            // Not used
        }
        public void OnError(SpeechRecognizerError error)
        {
            try
            {
                if (error == SpeechRecognizerError.NoMatch)
                {
                    // handle "could not catch, try again" error
                    speechRecognizer.Cancel();
                    speechRecognizer.StartListening(new Intent(RecognizerIntent.ActionRecognizeSpeech));
                }
                else if (error == SpeechRecognizerError.SpeechTimeout)
                {
                    // handle speech timeout error
                    callback.Invoke("Speech recognition timeout");
                }
                else if (error == SpeechRecognizerError.NetworkTimeout)
                {
                    // handle network timeout error
                    callback.Invoke("Network timeout");
                }
                else
                {
                    // handle other errors
                    callback.Invoke($"Error: {error.ToString()}");
                }
            }
           
            catch (Java.Lang.Exception e)
            {
                // handle exception
                callback.Invoke($"Error: {e.Message}");
            }
           
        }
        public void OnEvent(int eventType, Bundle @params)
        {
            // Not used
        }

        public void OnPartialResults(Bundle partialResults)
        {
            // Not used
        }

        public void OnReadyForSpeech(Bundle @params)
        {
            // Not used
        }

        public void OnRmsChanged(float rmsdB)
        {
            // Not used
        }

        public void OnResults(Bundle results)
        {
            throw new NotImplementedException();
        }
    }
}
