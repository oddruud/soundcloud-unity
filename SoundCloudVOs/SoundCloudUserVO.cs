using UnityEngine;
using SimpleJSON;
using System.Collections;

public class SoundCloudUserVO {
	
	public string Id;
	public string Username;
	public string AvatarUrl;
	public Texture2D AvatarImage;

	public static SoundCloudUserVO CreateFromJSON(JSONNode aUserJSON) {
		SoundCloudUserVO theUser = new SoundCloudUserVO();
		theUser.Id = aUserJSON["id"].Value;
		theUser.Username = aUserJSON["id"].Value;
		theUser.AvatarUrl = aUserJSON["avatar_url"].Value;
		return theUser;
	}

	public ImageLoader LoadAvatarImage() {
		if (AvatarUrl == null)
			return null;

		GameObject theAvatarLoaderObject = new GameObject();
		ImageLoader theAvatarLoader = theAvatarLoaderObject.AddComponent<ImageLoader>();
		theAvatarLoader.imageLoaded += HandleAvatarImageLoaded;
		theAvatarLoader.load(AvatarUrl);
		return theAvatarLoader;
	}

	void HandleAvatarImageLoaded (ImageLoader aImageLoader)
	{
		AvatarImage = aImageLoader.Image;
		aImageLoader.imageLoaded -= HandleAvatarImageLoaded;
	}
}
