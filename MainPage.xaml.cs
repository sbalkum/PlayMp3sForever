using CommunityToolkit.Maui.Views;
using System.Diagnostics;

#if ANDROID
using Android.Net.Wifi;
#endif

namespace PlayMp3sForever
{
    public partial class MainPage : ContentPage
    {
        int _count = 0;
        int _playCount = 0;

#if ANDROID
        WifiManager.WifiLock _wifiLock;
#endif

        List<string> _mp3Urls = new List<string>
        {
            "https://cdn.pixabay.com/audio/2022/05/02/audio_3641d5f7d1.mp3",
            "https://cdn.pixabay.com/audio/2023/08/31/audio_d2149da47a.mp3",
            "https://cdn.pixabay.com/audio/2024/07/29/audio_13d5362455.mp3",
            "https://cdn.pixabay.com/audio/2024/07/07/audio_0e2692984f.mp3",
            "https://cdn.pixabay.com/audio/2023/09/14/audio_7a3a069eba.mp3"
        };

        public MainPage()
        {
            InitializeComponent();

            _player.MediaEnded += (s, e) =>
            {
                SetMedia();
            };
            _player.MediaFailed += (s, e) =>
            {
                Debug.WriteLine($"Media failed ({_playCount}): {e.ErrorMessage}");
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

#if ANDROID
            var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);
            _wifiLock = wifiManager.CreateWifiLock(Android.Net.WifiMode.Full, "MyWifiLock");
            _wifiLock.SetReferenceCounted(false);
            _wifiLock.Acquire();
#endif

            SetMedia();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _player.Stop();
            _player.Handler?.DisconnectHandler();

#if ANDROID
            if (_wifiLock != null && _wifiLock.IsHeld) {
                _wifiLock.Release();
                _wifiLock = null;
            }
#endif
        }

        public void SetMedia()
        {
            var i = Random.Shared.Next(0, _mp3Urls.Count);
            var url = _mp3Urls[i];
            Debug.WriteLine("Setting media: " + url);

            Dispatcher.Dispatch(() =>
            {
                _player.Source = MediaSource.FromUri(url);
                _player.MetadataTitle = $"Title {DateTime.Now.Ticks}";
                _player.MetadataArtist = $"Artist {DateTime.Now.Ticks}";
                _player.MetadataArtworkUrl = "https://cdn.pixabay.com/photo/2016/03/29/10/38/piano-1287912_1280.png";

                _player.Play();

                _playCountLabel.Text = $"Played {++_playCount} times";
                Debug.WriteLine($"Play Count: {_playCount}");
            });
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            _count++;

            if (_count == 1)
                CounterBtn.Text = $"Clicked {_count} time";
            else
                CounterBtn.Text = $"Clicked {_count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
