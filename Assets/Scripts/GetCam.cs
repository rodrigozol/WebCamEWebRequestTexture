using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// https://stackoverflow.com/questions/24496438/can-i-take-a-photo-in-unity-using-the-devices-camera

public class GetCam : MonoBehaviour
{
    [SerializeField] WebCamTexture webCam;    
    //string your_path = "D:\\Lixo\\FotosCam";// any path you want to save your image 
    [SerializeField] string your_path = "";
    public RawImage display;
    public AspectRatioFitter fit;
    public int contadorFotos;
    //[SerializeField] GameObject planoWeb;

    IEnumerator Start()
    {
        your_path = "" + Application.dataPath;

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
           LigaCamComeco1();      
        }

    }

    void LigaCamComeco1() {
        if (WebCamTexture.devices.Length == 0)
        {
            //Debug.LogError("can not found any camera!");
            Debug.Log("can not found any camera!");
            return;
        }

        int index = -1;
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            if (WebCamTexture.devices[i].name.ToLower().Contains("pc"))
            {
                Debug.Log("WebCam Name:" + WebCamTexture.devices[i].name + "   Webcam Index:" + i);
                index = i;
            }
        }

        if (index == -1)
        {
            // Debug.LogError("can not found your camera name!");
             Debug.Log("can not found your camera name!");
            return;
        }

        WebCamDevice device = WebCamTexture.devices[index];
        webCam = new WebCamTexture(device.name);
        webCam.Play();
     
        display.texture = webCam;
       // planoWeb.GetComponent<Renderer>().materials[0].mainTexture = webCam;
    }

    
  

    public void Update()
    {
        if (webCam) {
           // Ratio();
        }        

        if (Input.GetKeyDown(KeyCode.PageUp)) {
          
            CallTakePhoto();
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            webCam.Stop();
            Invoke("ResetCena", 0.1f);
        }

    }


    void ResetCena() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("ResetCena_GetCam");
    }

    void Ratio() {
        float ratio = (float)webCam.width / (float)webCam.height;
        fit.aspectRatio = ratio;


        float ScaleY = webCam.videoVerticallyMirrored ? -1f : 1f;
        display.rectTransform.localScale = new Vector3(1f, ScaleY, 1f);

        int orient = -webCam.videoRotationAngle;
        display.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void CallTakePhoto() // call this function in button click event
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {
        yield return new WaitForEndOfFrame();      

        Texture2D photo = new Texture2D(webCam.width, webCam.height);
        photo.SetPixels(webCam.GetPixels());
        photo.Apply();

        /*
        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        File.WriteAllBytes(your_path + "\\photo.png", bytes);
        */

        //Encode to a JPG
        byte[] bytes = photo.EncodeToJPG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        File.WriteAllBytes(your_path + "\\ZaxisCam" + contadorFotos + ".jpg", bytes);
        contadorFotos++;
        Debug.Log("TirouFotoWebCam");

    }



}