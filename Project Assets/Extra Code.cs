

    /*void ShowSpots()
    {
        if(PlaceManagerScript.GetInstance() == null)
        {
            return;
        }

        if(_locatorImage1 != null)
        {
            Destroy(_locatorImage1.gameObject);
        }

        if (_locatorImage2 != null)
        {
            Destroy(_locatorImage2.gameObject);
        }

        if (PlaceManagerScript.GetInstance().GetCurrentPlace() == null)
        {
            return;
        }

        _locatorImage1 = Instantiate(_imagePrefab);

        _locatorImage1.sprite = _locator.GetAvatarSprite();

        _locatorImage1.GetComponent<RectTransform>().SetParent(_canvas.GetComponent<RectTransform>());

        _locatorImage1.GetComponent<RectTransform>().SetSiblingIndex(0);

        _locatorImage1.GetComponent<RectTransform>().position = _locator.GetAvatarScreenLocation();


        _locatorImage2 = Instantiate(_imagePrefab);

        _locatorImage2.sprite = _locator.GetTargetSprite();

        _locatorImage2.GetComponent<RectTransform>().SetParent(_canvas.GetComponent<RectTransform>());

        _locatorImage2.GetComponent<RectTransform>().SetSiblingIndex(0);

        _locatorImage2.GetComponent<RectTransform>().position = _locator.GetTargetScreenLocation();


    }*/
        [SerializeField]
    Image _imagePrefab;

    Image _locatorImage1;

    Image _locatorImage2;

     /*
 public void LocateWalker()
 {
     _pos1 = _cam.WorldToScreenPoint(_avatar.position);
 }

 public void LocateCurrentPlace()
 {
     if(PlaceManagerScript.GetInstance() == null)
     {
         return;
     }

     if(PlaceManagerScript.GetInstance().GetCurrentPlace() == null)
     {
         if(_destMarker != null)
         {
             GameObject.Destroy(_destMarker.gameObject);
         }

         return;
     }

     _target = PlaceManagerScript.GetInstance().GetCurrentPlace().gameObject.transform;

     _pos2 = _cam.WorldToScreenPoint(_target.position);
 }*/

 



    public void PlaceDestMarker()
    {
        if(_canvasTransform == null)
        {
            return;
        }

        if(PlaceManagerScript.GetInstance() == null)
        {
            return;
        }

        if(PlaceManagerScript.GetInstance().GetCurrentPlace() == null)
        {
            if(_destMarker != null)
            {
                GameObject.Destroy(_destMarker);
            }
            return;
        }

        LocateCurrentPlace();

        _destMarker = GameObject.Instantiate(_imagePrefab, _pos2, Quaternion.identity);

        _destMarker.GetComponent<RectTransform>().SetParent(_canvasTransform);

        _destMarker.GetComponent<RectTransform>().position = _pos2;

        _destMarker.GetComponent<RectTransform>().SetSiblingIndex(0);

        _destMarker.sprite = _targetSprite;
    }*/