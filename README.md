# soundclound-unity

Want to use soundcloud comments in your unity game? look no further. soundcloud-unity interfaces with the soundcloud http api and generates a SoundCloundTrack Value Object with comments.

# usage:

Attach the SoundCloudService behaviour to a game object. Fill in your SoundCloud developer clientId in the 'clientId' field.

Now load a track as such: 

```C#
public string TrackId; //the identifier of the track on SoundCloud. 

SoundCloudTrackLoader theLoader = _soundCloudService.createTrackLoader();
theLoader.TrackLoaded += HandleTrackLoaded;
theLoader.LoadingError += HandleLoadingError;
theLoader.load(TrackId, false);

```

Example TrackLoaded Handler:

```C#
protected void HandleLoadingError (SoundCloudTrackLoader aSoundCloudTrackLoader) {
	aSoundCloudTrackLoader.LoadingError -= HandleLoadingError;
	Debug.Log("failed to load soundcloud track: " + aSoundCloudTrackLoader.TrackId);
	Debug.Log(aSoundCloudTrackLoader.ErrorMessage);
		
	//Do something with this error
}
```

Example LoadingError Handler:

```C#
protected void HandleTrackLoaded (SoundCloudTrackLoader aSoundCloudTrackLoader) {
	aSoundCloudTrackLoader.TrackLoaded -= HandleTrackLoaded;
	//Do something with the aSoundCloudTrackLoader.soundCloudTrack Value Object.
}
```