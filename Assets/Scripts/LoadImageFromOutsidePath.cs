using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.SceneManagement;


public class LoadImageFromOutsidePath : MonoBehaviour
{
    [SerializeField] string pastaDeImagens;  
    [SerializeField] List<string> imagensLista = new List<string>();
    [SerializeField] List<string> imagensListaNomeLimpa = new List<string>();
    [SerializeField] GameObject planoMostraImagens;
    [SerializeField] int contadorImagens;
    [SerializeField] string fotoNomeLimpa;
    [SerializeField] TMP_Text textoPicture;
    private DirectoryInfo dirImagens;
    [SerializeField] TMP_InputField fieldPasta; 
    

    // Start is called before the first frame update
    void Start()
    {
        AcharGuis();
        LoadPrefs();
        PegaImagensPastaEMontaArrays();
        contadorImagens = 0;
    }

    public void AcharGuis() {
        GameObject textoPictureTemp = GameObject.Find("Text (TMP)_textoPicture");
        textoPicture = textoPictureTemp.GetComponent<TMP_Text>();
        GameObject tempFieldPasta = GameObject.Find("InputField (TMP)_PastaImagens");
        fieldPasta = tempFieldPasta.GetComponent<TMP_InputField>();

        Debug.Log("BuscandoGuis");
    }

    void LoadPrefs() {
        if (!PlayerPrefs.HasKey("PastaImagensPath"))
        {
            //fieldPasta.text = "C:/Users/Public/Pictures";
            fieldPasta.text = "D:/ZaxisTools/Material_Visual/Tapete_Danca";


        }
        else
        {
            fieldPasta.text = PlayerPrefs.GetString("PastaImagensPath");
        }

        pastaDeImagens = fieldPasta.text;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            LoadImagesStart();
        }
     
        if (imagensLista.Count > 0) {
         
            if (contadorImagens >= imagensLista.Count)
            {
                contadorImagens = 0;
            }
        }
       
    }

    public void LoadImagesStart() {
        if (contadorImagens < imagensLista.Count)
        {
            StartCoroutine(LoadImagemEspecifica(imagensLista[contadorImagens], planoMostraImagens));
        }

      

        contadorImagens++;

        if (contadorImagens > 0)
        {
            textoPicture.text = imagensListaNomeLimpa[contadorImagens - 1];
            //textoPicture.text = imagensListaNomeLimpa[contadorImagens];
        }
    }


    void PegaImagensPastaEMontaArrays() {
       // dirImagens = new DirectoryInfo(pastaDeImagens); // pegando em qualquer pasta 
       dirImagens = new DirectoryInfo(Application.dataPath); // pegando na raizProjeto Windows;
      //  dirImagens = new DirectoryInfo(Application.persistentDataPath); // pegando na raiz do projeto Android e C:User;


        FileInfo[] infoImagens = dirImagens.GetFiles("*.jpg");       

        if (infoImagens.Length > 0)
        {
            Debug.Log("MontandoPastas");
                    
            imagensLista = new List<string>(new string[infoImagens.Length]);
            imagensListaNomeLimpa = new List<string>(new string[infoImagens.Length]);

            for (int z = 0; z < infoImagens.Length; z++)
            {            
                imagensLista[z] = infoImagens[z].FullName;
                imagensListaNomeLimpa[z] = infoImagens[z].Name;                
            }

            //limpando nome das imagens
            for (var i = 0; i < imagensListaNomeLimpa.Count; i++)
            {
                imagensListaNomeLimpa[i] = imagensListaNomeLimpa[i].Split('.')[0];
             
            }
          
        }

        LimpaFotoCam();

    }

    void LimpaFotoCam() {

        Debug.Log("LimpandoListadeImagens_TirandoFotoCam");
        for (var i = 0; i < imagensLista.Count; i++) {
            if (imagensLista[i].Contains("ZaxisCam")) {
                Debug.Log("Achou_AlgumaFotoCam");
                imagensLista.RemoveAt(i);
                imagensListaNomeLimpa.RemoveAt(i);
            }
        }

        for (var i = 0; i < imagensLista.Count; i++) {
            if (imagensLista[i].Contains("ZaxisCam")) {
                LimpaFotoCam();
            }
        }

    }
    public IEnumerator LoadImagemEspecifica(string imagemPedida, GameObject superficie) {

        Debug.Log("Endereco_Imagem: " + imagemPedida);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imagemPedida);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else {
            Debug.Log("AchouImagem");
            var tempTexture = DownloadHandlerTexture.GetContent(request);
            //planoMostraImagens.GetComponent<Renderer>().material.mainTexture = tempTexture;
            superficie.GetComponent<Renderer>().materials[0].mainTexture = tempTexture; // pode mudar index caso objeto tenha mais de uma material    
         


        }
    }

    public void SalvaPasta() {
        PlayerPrefs.SetString("PastaImagensPath", fieldPasta.text);
        pastaDeImagens = fieldPasta.text;

        ResetArrayLista();
    }

    void ResetArrayLista() {      
        imagensLista = new List<string>(new string[0]);
        imagensListaNomeLimpa = new List<string>(new string[0]);
        PegaImagensPastaEMontaArrays();
        contadorImagens = 0;
    }


}


