namespace Infrastructure.ServiceInterfaces
{
    public interface IGameSettingsManager
    {
        bool MouseVisibility { get; set; }

        bool AllowWindowResize { get; set; }

        bool FullScreen { get; set; }

        int NumOfPlayers { get; set; }

        float BackgroundVolume { get; set; }

        float SoundEffectsVolume { get; set; }

        bool MusicToggle { get; set; }
    }
}
