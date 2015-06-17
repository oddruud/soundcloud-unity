using UnityEngine;
using SimpleJSON;
using System.Collections;

public delegate void ImageLoadedEventHandler(ImageLoader aImageLoaded);

public class ImageLoader : MonoBehaviour {

	protected string _imageURL;
	protected Texture2D _image;
	protected event ImageLoadedEventHandler _imageLoaded;
	
	public Texture2D Image {
		get{return _image;}
	}

	public string SourceUrl {
		get{return _imageURL;}
	}

	public event ImageLoadedEventHandler imageLoaded
	{
		add
		{
			_imageLoaded += value;
		}
		remove
		{
			_imageLoaded -= value;
			
			//nobody is listening to the loader anymore, lets delete it
			if (_imageLoaded == null) {
				GameObject.Destroy(gameObject);
			}
		}
	}
	
	public void load(string aUrl) {
		_imageURL = aUrl;
		_image = new Texture2D(128, 128, TextureFormat.DXT1, true);
		StartCoroutine(loadImage(_imageURL));
	}
	
	IEnumerator loadImage(string aUrl) {

		WWW loader = new WWW(aUrl);
		yield return loader;

		loader.LoadImageIntoTexture(_image);

		if (_imageLoaded != null)
			_imageLoaded(this);
	}
}
