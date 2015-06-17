using UnityEngine;
using SimpleJSON;
using System.Collections;

public class SoundCloudCommentVO {
	
	public string id;
	public string trackId;
	public string createdAt;
	public int timestamp;
	public string message;
	public SoundCloudUserVO User;
	
	public SoundCloudCommentVO() {}
	
	public static SoundCloudCommentVO CreateFromJSON(JSONNode aCommentJSON) {
		SoundCloudCommentVO theComment = new SoundCloudCommentVO();
		theComment.id = aCommentJSON["id"].Value;
		theComment.trackId = aCommentJSON["track_id"].Value;
		theComment.createdAt = aCommentJSON["created_at"].Value;
		theComment.timestamp =  aCommentJSON["timestamp"].AsInt;
		theComment.message = aCommentJSON["body"].Value;
		theComment.User = SoundCloudUserVO.CreateFromJSON(aCommentJSON["user"]);
		return theComment;
	}

	public ImageLoader LoadAvatarImage() {
		if (User != null && User.AvatarUrl != null)
			return User.LoadAvatarImage();

		return null;
	}

}
