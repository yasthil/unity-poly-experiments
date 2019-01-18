using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using UnityEngine.UI;

public class ModelLoader : MonoBehaviour
{
    public int assetCount = 0;
    public GameObject _parentContainer;
    public InputField _inputField;


    private static ModelLoader _instance;
    private List<GameObject> _assetsLoaded;
    private float _symbolYOffset = 0.5f;
    private int _noOfAssetsToRuquest = 1;

    public static ModelLoader Instance
    {
        get
        {
            return _instance;
        }      
    }

    private void Awake()
    {
        // Singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        _assetsLoaded = new List<GameObject>();       
    }
    private void ClearAssets()
    {
        for (int i = 0; i < _assetsLoaded.Count; i++)
        {
            Destroy(_assetsLoaded[i]);
        }
        _assetsLoaded.Clear();
    }

    public void Search()
    {
        ClearAssets();

        // Create a new request
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        req.orderBy = PolyOrderBy.BEST;

        // Search by keywords    
        if(_inputField.text == "")
        {
            _inputField.text = "dog";
        }
        req.keywords = _inputField.text;
        

        // Make the request with a callback function
        PolyApi.ListAssets(req, GetSearchResults);
    }

    private void GetSearchResults(PolyStatusOr<PolyListAssetsResult> result)
    {
        // Set options for import so the assets aren't crazy sizes
        PolyImportOptions options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 1.0f;
        options.recenter = true;

        // List our assets
        List<PolyAsset> assetsInUse = new List<PolyAsset>();

        // Loop through the list and display the first x
        for (int i = 0; i < Mathf.Min(_noOfAssetsToRuquest, result.Value.assets.Count); i++)
        {
            // Import our assets into the scene with the ImportDonuts function
            PolyApi.Import(result.Value.assets[i], options, StoreResults);

            assetsInUse.Add(result.Value.assets[i]);
        }

    }

    /// <summary>
    /// Stores the results.
    /// </summary>
    /// <param name="asset">Asset.</param>
    /// <param name="result">Result.</param>
    private void StoreResults(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        assetCount++;
        result.Value.gameObject.transform.position = new Vector3(0, _symbolYOffset, 0f);
        result.Value.gameObject.SetActive(false);
        _assetsLoaded.Add(result.Value.gameObject);
    }


    public void LoadAssetsIntoContainer()
    {
        GameObject iAsset;
        for (int i = 0; i < _assetsLoaded.Count; i++)
        {
            iAsset = _assetsLoaded[i];
            iAsset.SetActive(true);
            iAsset.transform.SetParent(_parentContainer.transform);

        }
    }
}
