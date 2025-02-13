using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingInhalerGameScript : MatchingGameCanvasScript
{
    [Header("3. Variables for the Building Game")]

    [SerializeField]
    HighlightingAnimationClass _highlightingAnimationProperties;

    [SerializeField]
    float _arrowContainerSize = 2.0f;

    [SerializeField]
    float _arrowSize = 2.0f;

    [SerializeField]
    float _numberModelSize = 2.0f;

    [SerializeField]
    List<ArrowAnimationClass> _arrowAnimations;

    [SerializeField]
    List<ArrowAnimationClass> _instancedArrowAnimations;

    [SerializeField]
    PlaceIndicatorScript _placeIndicator;

    ArrowAnimationClass _currentInstancedArrowAnimation;

    Quaternion _gameSpaceInitialRotation;

    [ContextMenu("Add Components.")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new InhalerMatchingBlockAndHoleClass());
    }

    [SerializeField]
    GameObject _cap;

    int _currentPhaseNumber = 0;

    Coroutine _animationCoroutineArrow;

    Coroutine _animationCoroutineHighlight;

    Vector3 _capLocalPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(_instancedArrowAnimations == null)
        {
            _instancedArrowAnimations = new List<ArrowAnimationClass>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentGame == this)
        {
            IUpdateExperience();            
        }
    }

    public HighlightingAnimationClass GetHighlightingAnimationProperties()
    {
        return _highlightingAnimationProperties;
    }

    public int GetCurrentPhaseNumber()
    {
        return _currentPhaseNumber;
    }

    public List<ArrowAnimationClass> GetArrowAnimations()
    {
        return _arrowAnimations;
    }

    public List<ArrowAnimationClass> GetInstancedArrowAnimations()
    {
        return _instancedArrowAnimations;
    }

    public ArrowAnimationClass GetCurrentInstancedArrowAnimation()
    {
        return _currentInstancedArrowAnimation;
    }

    public override void IStartExperience()
    {
        if(_currentGame != null || _gameSpace == null || _spawningArea == null || _floor == null || _mainContainer == null)
        {
            return;
        }

        base.IStartExperience();

        _currentPhaseNumber = 0;

        Quaternion _currentHoleRotation;

        Vector3 _currentHolePosition;

        List<Vector3> _blockPositions = new List<Vector3>();

        _floor.SetActive(true);

        _walls.SetActive(true);

        SetBadge(BadgesManagerScript.GetInstance().GetBadgeByName("Inhaler Assembly Badge"));

        SetResponseTextProperties(-551.0f, 287.0f);

        for(int _i = 0; _i < _presetBlocksAndHoles.Count; _i++)
        {
            InhalerMatchingBlockAndHoleClass _currentPreset = _presetBlocksAndHoles[_i] as InhalerMatchingBlockAndHoleClass;

            if (_currentPreset == null)
            {
                Debug.LogError("This is not a valid type of preset.");

                continue;
            }

            if (_currentPreset.GetGameBlock() == null || _currentPreset.GetGameBlockHole() == null)
            {
                Debug.LogError("No block or hole to instantiate.");

                continue;
            }

            //1. Instantiating Block

            GameObject _newBlock = Instantiate(_currentPreset.GetGameBlock().gameObject);

            if(_currentPreset.GetMatchingAttribute() == "Inhaler Neck")
            {
                GetCap(_newBlock);
            }

            _newBlock.transform.parent = _spawningArea;

            base.FindSpawningSpot(ref _blockPositions, ref _newBlock);

            _newBlock.transform.parent = _mainContainer;

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetMatchingGameCanvas(this);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetMaintainZDistance(false);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetOffset_Z(_draggingZOffset);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetApplyGravity(true);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetObjectName(_currentPreset.GetMatchingAttribute());

            _gameProperties.AddObjectToList(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _gameProperties.AddObjectsAsGO(_newBlock);

            _newBlock.GetComponent<DraggableClass>().SetCamera(_camera);

            _newBlock.GetComponent<DraggableClass>().GetBody().velocity = Vector3.zero;

            _newBlock.transform.localScale = (Vector3.one * _spawningSizeForBlocks);

            _newBlock.SetActive(true);

            //2. Instantiating Hole

            GameObject _newHole = Instantiate(_currentPreset.GetGameBlockHole().gameObject);

            _newHole.transform.parent = null;

            _currentHolePosition = _currentPreset.GetGameBlockHole().gameObject.transform.position;

            _currentHoleRotation = _currentPreset.GetGameBlockHole().gameObject.transform.rotation;

            _newHole.transform.position = _currentHolePosition;

            _newHole.transform.rotation = _currentHoleRotation;

            _newHole.transform.parent = _gameSpace.transform;

            _newHole.transform.localScale = Vector3.one;

            InhalerMatchingObjectHoleScript _holeInfo = _newHole.GetComponent<InhalerMatchingObjectHoleScript>();

            DestroyArrowLocations(_newHole);

            _currentHoleProperties.AddObjectsAsGO(_newHole);

            _newHole.SetActive(_i == 0);

            try
            {
                if (_holeInfo != null)
                {
                    InhalerInformationClass _currentInfo;

                    _holeInfo.SetHoleGameCanvas(this);

                    _currentInfo = InhalerManagerScript.GetInstance().GetInhalerInfoList()[_i];

                    _holeInfo.SetCamera(_camera);

                    _holeInfo.SetHoleName(_currentPreset.GetMatchingAttribute());

                    _currentHoleProperties.AddObjectToList(_holeInfo);

                    _holeInfo.SetCorrectMatchDialogue("");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("The error occurs at index '" + _i + "'.");
            }
        }

        GameObject _go, _arrow3dObject, _number3dObject;

        int _numberModel;

        string _numberModelName;

        Transform _p;

        for(int _i = 0;  _i < _arrowAnimations.Count; _i++)
        {
            ArrowAnimationClass _currentAnim = _arrowAnimations[_i];

            if(_currentAnim.GetArrowContainer() == null || _currentAnim.GetArrowObject() == null)
            {
                continue;
            }

            _numberModel = _i + 1;

            ArrowAnimationClass _newAnim = new ArrowAnimationClass();

            _newAnim.SetPointA(_currentAnim.GetPointA());

            _newAnim.SetPointB(_currentAnim.GetPointB());

            _newAnim.SetSpaceType(_currentAnim.GetSpaceType());

            _newAnim.SetAnimationSpeed(_currentAnim.GetAnimationSpeed());            

            _go = Instantiate(_currentAnim.GetArrowContainer(), _currentAnim.GetArrowContainer().transform.position, _currentAnim.GetArrowContainer().transform.rotation);

            //SetArrowContainerGeometry(_currentAnim.GetArrowContainer(), _go);

            _go.transform.parent = _gameSpace.transform;

            _go.transform.localScale = Vector3.one * _arrowContainerSize;

            _newAnim.SetArrowContainer(_go);

            _newAnim.GetArrowContainer().SetActive(false);

            _instancedArrowAnimations.Add(_newAnim);

            _arrow3dObject = _go.transform.Find("Arrow 3D Object").gameObject;

            _p = _arrow3dObject.transform.parent;

            _arrow3dObject.transform.parent = _sceneContainer.transform;

            _arrow3dObject.transform.localScale = Vector3.one * _arrowSize;

            _arrow3dObject.transform.parent = _p;

            _newAnim.SetArrowObject(_arrow3dObject);

            _numberModelName = "Number " + _numberModel.ToString() + " Model";

            _number3dObject = _go.transform.Find(_numberModelName).gameObject;

            _p = _number3dObject.transform.parent;

            _number3dObject.transform.parent = _sceneContainer;

            _number3dObject.transform.localScale = Vector3.one * _numberModelSize;

            _number3dObject.transform.parent = _p;

            _newAnim.SetNumber3DObject(_number3dObject);

            /*

            _arrow3dObject = _go.transform.Find("Arrow 3D Object").gameObject;

            _newAnim.SetArrowObject(_arrow3dObject);

            _newAnim.GetArrowContainer().SetActive(false);

            _instancedArrowAnimations.Add(_newAnim);

            //_go.transform.parent = _mainContainer.transform;

            _arrow3dObject.transform.localScale = Vector3.one * _arrowSize;

            _numberModelName = "Number " + _numberModel.ToString() + " Model";

            _number3dObject.transform.localScale = Vector3.one * _numberModelSize;*/
        }

        _completionMeter.SetMaxValue(4);

        _completionMeter.SetValue(0);

        _completionMeter.SignalToUpdateUI();

        _instancedArrowAnimations[0].GetArrowContainer().SetActive(true);

        _instancedArrowAnimations[0].GetNumber3DObject().SetActive(true);

        _currentInstancedArrowAnimation = _instancedArrowAnimations[0];

        _animationCoroutineArrow = StartCoroutine(_instancedArrowAnimations[0].Animate());

        SetResponseText("So, you want to help Fasty assemble the inhaler? Very well! Let's get started, my friend!\n\n1. Find the neck and place it on the shaded area which is pointed by the green arrow.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f, -1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f));

        _gameSpaceInitialRotation = _gameSpace.transform.rotation;

        if (_dialogues != null)
        {
            _dialogues.PlayClip("Dr. Salem Assembly Game Step 1");
        }

        _responseText.fontSize = 50;
    }

    void GetCap(GameObject _input)
    {
        if(_input == null)
        {
            return;
        }

        Transform _tr = _input.transform;

        _tr = _tr.Find("Cap 2");

        if(_tr == null)
        {
            return;
        }

        _cap = _tr.gameObject;

        if(_cap.GetComponent<MeshCollider>() != null)
        {
            _cap.GetComponent<MeshCollider>().enabled = false;
        }

        if(_cap.GetComponent<DraggableClass>() != null)
        {
            _cap.GetComponent<DraggableClass>().SetDraggableOn(false);

            _cap.GetComponent<DraggableClass>().SetCamera(_camera);
        }

        _capLocalPosition = _cap.transform.localPosition;

        _cap.SetActive(true);
    }

    public override void IUpdateExperience()
    {
        if (_currentPhaseNumber == 0 && _currentHoleProperties.GetListOfObjects()[0].GetObjectPlaced())
        {
            Debug.Log("In the building game, we are entering phase 2.");

            _currentHole = _currentHoleProperties.GetListOfObjects()[0];

            _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.0f);

            _currentHole = _currentHoleProperties.GetListOfObjects()[1];

            _currentHole.gameObject.SetActive(true);

            _currentPhaseNumber = 1;

            _instancedArrowAnimations[0].SetAnimateBoolean(false);

            if (_animationCoroutineArrow != null)
            {
                StopCoroutine(_animationCoroutineArrow);
            }

            _instancedArrowAnimations[0].GetArrowContainer().SetActive(false);

            _instancedArrowAnimations[0].GetNumber3DObject().SetActive(false);

            _instancedArrowAnimations[1].GetArrowContainer().SetActive(true);

            _instancedArrowAnimations[1].GetNumber3DObject().SetActive(true);

            _currentInstancedArrowAnimation = _instancedArrowAnimations[1];

            _animationCoroutineArrow = StartCoroutine(_instancedArrowAnimations[1].Animate());

            SetResponseText("Well Done!\n\n2. Now, find the medicine, and drag it on the shaded area above the neck.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f, -1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f));

            if (_dialogues != null)
            {
                _dialogues.PlayClip("Dr. Salem Assembly Game Step 2");
            }

            _completionMeter.SignalToUpdateUI();
        }

        if (_currentPhaseNumber == 1 && _currentHoleProperties.GetListOfObjects()[1].GetObjectPlaced())
        {
            _currentHole = _currentHoleProperties.GetListOfObjects()[1];

            _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.0f);

            TurnOnCapComponent();

            _currentPhaseNumber = 2;

            _instancedArrowAnimations[1].SetAnimateBoolean(false);

            if (_animationCoroutineArrow != null)
            {
                StopCoroutine(_animationCoroutineArrow);
            }

            _instancedArrowAnimations[1].GetArrowContainer().SetActive(false);

            _instancedArrowAnimations[1].GetNumber3DObject().SetActive(false);

            _instancedArrowAnimations[2].GetArrowContainer().SetActive(true);

            _instancedArrowAnimations[2].GetNumber3DObject().SetActive(true);

            _animationCoroutineArrow = StartCoroutine(_instancedArrowAnimations[2].Animate());

            _currentInstancedArrowAnimation = _instancedArrowAnimations[2];

            _completionMeter.SignalToUpdateUI();

            if (_dialogues != null)
            {
                _dialogues.PlayClip("Dr. Salem Assembly Game Step 3");
            }

            SetResponseText("You're doing great!\n\n3. Remove the cap from the neck.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f, -1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f));


            if (_arrowAnimations[1].GetArrowContainer().activeSelf)
            {
                _arrowAnimations[1].GetArrowContainer().SetActive(false);
            }
        }

        if (_currentPhaseNumber == 2 && _cap != null)
        {
            if (_cap.GetComponent<DraggableClass>() != null)
            {
                if (_cap.GetComponent<DraggableClass>().GetTouchPhase() == TouchPhase.Ended && _cap.transform.localPosition.z >= 0.035f)
                {
                    _currentHole = _currentHoleProperties.GetListOfObjects()[2];

                    _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.7f);

                    _currentHole.gameObject.SetActive(true);

                    _currentPhaseNumber = 3;

                    _highlightingAnimationProperties.SetAnimateBoolean(false);

                    if (_animationCoroutineHighlight != null)
                    {
                        StopCoroutine(_animationCoroutineHighlight);
                    }

                    _cap.layer = 0;

                    _cap.SetActive(false);

                    _instancedArrowAnimations[2].SetAnimateBoolean(false);

                    if (_animationCoroutineArrow != null)
                    {
                        StopCoroutine(_animationCoroutineArrow);
                    }

                    _instancedArrowAnimations[2].GetArrowContainer().SetActive(false);

                    _instancedArrowAnimations[2].GetNumber3DObject().SetActive(false);

                    _instancedArrowAnimations[3].GetArrowContainer().SetActive(true);

                    _instancedArrowAnimations[3].GetNumber3DObject().SetActive(true);

                    _animationCoroutineArrow = StartCoroutine(_instancedArrowAnimations[3].Animate());

                    _currentInstancedArrowAnimation = _instancedArrowAnimations[3];

                    if (_dialogues != null)
                    {
                        _dialogues.PlayClip("Dr. Salem Assembly Game Step 4");
                    }

                    _completionMeter.AddToValue(1);

                    SetResponseText("We're almost done...\n\n4. Find the spacer and place it on the shaded area pointed by the arrow.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f, -1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f));

                    if (_correctAudioClip != null)
                    {
                        AudioSource _as = _currentHoleProperties.GetListOfObjects()[0].GetAudioSource();

                        if (_as != null)
                        {
                            _as.clip = _correctAudioClip;

                            _as.Play();
                        }
                    }

                    IGameCorrect();

                    _completionMeter.SignalToUpdateUI();
                }
                else if (_cap.GetComponent<DraggableClass>().GetTouchPhase() == TouchPhase.Ended)
                {
                    _cap.transform.localPosition = _capLocalPosition;

                    if (_incorrectAudioClip != null)
                    {
                        AudioSource _as = _currentHoleProperties.GetListOfObjects()[0].GetAudioSource();

                        if (_as != null)
                        {
                            _as.clip = _incorrectAudioClip;

                            _as.Play();
                        }
                    }

                    IGameIncorrect();
                }
            }
        }

        if (_currentPhaseNumber == 3 && _currentHoleProperties.GetListOfObjects()[2].GetObjectPlaced())
        {
            _instancedArrowAnimations[3].SetAnimateBoolean(false);

            if (_animationCoroutineArrow != null)
            {
                StopCoroutine(_animationCoroutineArrow);
            }

            _instancedArrowAnimations[3].GetArrowContainer().SetActive(false);

            _instancedArrowAnimations[3].GetNumber3DObject().SetActive(false);

            if (_dialogues != null)
            {
                _dialogues.PlayClip("Dr. Salem Assembly Game Final");
            }

            _gameCanvas.GetQuitButton().gameObject.SetActive(false);

            StartCoroutine(IWaitUntilCompletion());

            _currentPhaseNumber = -1;

            _currentInstancedArrowAnimation = null;
        }

        LookIntoCamera();

        _completionMeter.UpdateUI();
    }

    public override void IStopExperience()
    {
        base.IStopExperience();

        if(_currentInstancedArrowAnimation != null)
        {
            _currentInstancedArrowAnimation.SetAnimateBoolean(false);

            _currentInstancedArrowAnimation = null;
        }

        if(_cap != null)
        {
            _cap.transform.localPosition = _capLocalPosition;
        }

        _gameSpace.transform.rotation = _gameSpaceInitialRotation;

        _currentPhaseNumber = 0;

        _floor.SetActive(false);

        _walls.SetActive(false);

        if(_animationCoroutineArrow != null)
        {
            StopCoroutine(_animationCoroutineArrow);
        }

        if(_animationCoroutineHighlight != null)
        {
            StopCoroutine(_animationCoroutineHighlight);
        }

        foreach(ArrowAnimationClass _anim in _instancedArrowAnimations)
        {
            Destroy(_anim.GetArrowContainer());

            Destroy(_anim.GetNumber3DObject());
        }

        _instancedArrowAnimations.Clear();

        _gameProperties.ClearObjectLists();

        _currentHoleProperties.ClearObjectLists();

        if (_currentGame == this)
        {
            _currentGame = null;
        }
    }

    public override void IChooseToQuitExperience()
    {
        if (_currentInstancedArrowAnimation != null)
        {
            _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _currentInstancedArrowAnimation.SetPauseAnimation(false); });

            _currentInstancedArrowAnimation.SetPauseAnimation(true);
        }

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { base.IResumeExperience(); });

        _yesOrNoCanvas.GetYesButton().onClick.AddListener(delegate { IStopExperience(); });

        _informationCanvas.gameObject.SetActive(false);

        base.IChooseToQuitExperience();
    }

    void TurnOnCapComponent()
    {
        if(_cap == null)
        {
            return;
        }

        if (_cap.GetComponent<MeshCollider>() != null)
        {
            _cap.GetComponent<MeshCollider>().enabled = true;
        }

        if (_cap.GetComponent<DraggableClass>() != null)
        {
            _cap.GetComponent<DraggableClass>().SetDraggableOn(true);
        }

        _cap.layer = 8;

        _animationCoroutineHighlight = StartCoroutine(_highlightingAnimationProperties.Animate());
    }

    public override IEnumerator IWaitUntilCompletion()
    {
        SetResponseText("Excellent! We now have a complete inhaler!", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f, -1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f));

        yield return new WaitForSeconds(5.0f);

        base.ICompleteExperience();

        _currentHoleProperties.ClearObjectLists();

        _currentPhaseNumber = 0;
    }

    protected override void LookIntoCamera()
    {
        if (_gameSpace == null || _camera == null || _currentGame == null)
        {
            return;
        }

        if (_currentGame != this)
        {
            return;
        }

        var _lookPosCam = _camera.gameObject.transform.position - _gameSpace.transform.position;

        _lookPosCam.y = 0.0f;

        var _rot = Quaternion.LookRotation(_lookPosCam);

        _rot = _rot * Quaternion.Euler(_additionalLookingAngles);

        bool _notHolding = true;

        if (DraggableManagerClass.GetInstance() != null)
        {
            _notHolding = !DraggableManagerClass.GetInstance().GetDraggingSomething();
        }

        if (_notHolding)
        {
            //_gameSpace.transform.rotation = Quaternion.Slerp(_gameSpace.transform.rotation, _rot, Time.deltaTime);

            _gameSpace.transform.rotation = _rot;
        }
    }

    public override void IGameCorrect()
    {
        base.IGameCorrect();
    }

    public override void IGameIncorrect()
    {
        _dialogues.PlayClip("Dr. Salem Assembly Game Not Right");

        base.IGameIncorrect();
    }

    void DestroyArrowLocations(GameObject _input)
    {
        if(_input == null)
        {
            return;
        }

        List<GameObject> _arrowLocations = new List<GameObject>();

        Transform _tr;

        for(int _i = 0; _i < _input.transform.childCount; _i++)
        {
            _tr = _input.transform.GetChild(_i);

            if (_tr == null)
            {
                continue;
            }

            if(_tr.gameObject.tag == "Arrow Location Tag")
            {
                _arrowLocations.Add(_tr.gameObject);
            }
        }

        string _name;

        foreach(GameObject _go in _arrowLocations)
        {
            _name = _go.name;

            Destroy(_go);

            Debug.Log(@"""" + _name + @"""" + " has been destroyed.");
        }
    }

    void SetArrowContainerGeometry(GameObject _originalContainerInput, GameObject _copiedContainerInput)
    {
        if(_originalContainerInput == null || _copiedContainerInput == null)
        {
            return;
        }

        GameObject _copy = Instantiate(_originalContainerInput);

        _copy.transform.parent = null;

        Vector3 _position = _copy.transform.position;

        Quaternion _q = _copy.transform.rotation;

        Vector3 _scale = _copy.transform.localScale;

        _copiedContainerInput.transform.parent = null;

        _copiedContainerInput.transform.position = _position;

        _copiedContainerInput.transform.rotation = _q;

        _copiedContainerInput.transform.localScale = _scale;

        Destroy(_copy);
    }
}
