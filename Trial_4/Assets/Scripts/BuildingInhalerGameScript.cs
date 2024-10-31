using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingInhalerGameScript : MatchingGameCanvasScript
{
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

    ArrowAnimationClass _currentInstancedArrowAnimation;

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
            UpdateStatus();

            _gameProperties.UpdateUI();
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

    public override void StartGame()
    {
        if(_currentGame != null || _gameSpace == null || _spawningArea == null || _floor == null || _mainContainer == null)
        {
            return;
        }

        base.StartGame();

        _currentPhaseNumber = 0;

        Quaternion _currentHoleRotation;

        Vector3 _currentHolePosition;

        List<Vector3> _blockPositions = new List<Vector3>();

        _floor.SetActive(true);

        _gameProperties.SetBadge(BadgesManagerScript.GetInstance().GetBadgeByName("Inhaler Assembly Badge"));

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

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetApplyGravity(true);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetObjectName(_currentPreset.GetMatchingAttribute());

            _gameProperties.AddObjectToList(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _gameProperties.AddObjectsAsGO(_newBlock);

            _currentBlocksAndHoles.AddBlock(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _newBlock.GetComponent<DraggableClass>().SetCamera(_gameProperties.GetCamera());

            _newBlock.GetComponent<DraggableClass>().GetBody().velocity = Vector3.zero;

            _newBlock.transform.localScale = (Vector3.one * _spawningSizeForBlocks);

            _newBlock.SetActive(true);

            //2. Instantiating Hole

            GameObject _newHole = Instantiate(_currentPreset.GetGameBlockHole().gameObject);

            _currentHolePosition = _currentPreset.GetGameBlockHole().gameObject.transform.position;

            _currentHoleRotation = _currentPreset.GetGameBlockHole().gameObject.transform.rotation;

            _newHole.transform.position = _currentHolePosition;

            _newHole.transform.rotation = _currentHoleRotation;

            _newHole.transform.parent = _gameSpace.transform;

            _newHole.transform.localScale = Vector3.one;

            InhalerMatchingObjectHoleScript _holeInfo = _newHole.GetComponent<InhalerMatchingObjectHoleScript>();

            Destroy(_newHole.transform.Find("Arrow Location").gameObject);

            if(_i == 0)
            {
                _newHole.SetActive(true);

                Destroy(_newHole.transform.Find("Arrow Location 2").gameObject);
            }
            else
            {
                _newHole.SetActive(false);
            }

            try
            {
                if (_holeInfo != null)
                {
                    InhalerInformationClass _currentInfo;

                    _holeInfo.SetHoleGameCanvas(this);

                    _currentInfo = InhalerManagerScript.GetInstance().GetInhalerInfoList()[_i];

                    _holeInfo.SetCamera(_gameProperties.GetCamera());

                    _holeInfo.SetHoleName(_currentPreset.GetMatchingAttribute());

                    _currentBlocksAndHoles.AddHole(_holeInfo);
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

            _go = Instantiate(_currentAnim.GetArrowContainer());

            _go.transform.position = _currentAnim.GetArrowContainer().transform.position;

            _go.transform.rotation = _currentAnim.GetArrowContainer().transform.rotation;

            _newAnim.SetArrowContainer(_go);

            _arrow3dObject = _go.transform.Find("Arrow 3D Object").gameObject;

            _newAnim.SetArrowObject(_arrow3dObject);

            _newAnim.GetArrowContainer().SetActive(false);

            _instancedArrowAnimations.Add(_newAnim);

            _go.transform.parent = _mainContainer.transform;

            _arrow3dObject.transform.localScale = Vector3.one * _arrowSize;

            _numberModelName = "Number " + _numberModel.ToString() + " Model";

            _number3dObject = _go.transform.Find(_numberModelName).gameObject;

            _newAnim.SetNumber3DObject(_number3dObject);

            _number3dObject.transform.localScale = Vector3.one * _numberModelSize;
        }

        _gameProperties.GetMeter().SetMaxValue(4);

        _gameProperties.GetMeter().SetValue(0);

        _gameProperties.SignalToUpdateUI();

        _instancedArrowAnimations[0].GetArrowContainer().SetActive(true);

        _instancedArrowAnimations[0].GetNumber3DObject().SetActive(true);

        _currentInstancedArrowAnimation = _instancedArrowAnimations[0];

        _animationCoroutineArrow = StartCoroutine(_instancedArrowAnimations[0].Animate());

        _gameProperties.SetResponseText("So, you want to help Fasty assemble the inhaler? Very well! Let's get started, my friend!\n\n1. Find the neck and place it on the shaded area which is pointed by the green arrow.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));

        _gameProperties.GetResponseText().fontSize = 50;
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

            _cap.GetComponent<DraggableClass>().SetCamera(_gameProperties.GetCamera());
        }

        _capLocalPosition = _cap.transform.localPosition;
    }

    void UpdateStatus()
    {
        MatchingGameHoleScript _currentHole;

        //_currentBlocksAndHoles

        /*if(DraggableManagerClass.GetInstance() != null && _currentBlocksAndHoles.GetSelectedHole() != null)
        {
            if(DraggableManagerClass.GetInstance().GetCurrentlyDraggedObject() != null)
            {
                MatchingGameBlockScript _block = DraggableManagerClass.GetInstance().GetCurrentlyDraggedObject().gameObject.GetComponent<MatchingGameBlockScript>();

                bool _dragEnded = DraggableManagerClass.GetInstance().GetTouchPhase() == TouchPhase.Ended;

                if(_block != null && _dragEnded)
                {
                    _currentHole = _currentBlocksAndHoles.GetSelectedHole();

                    if(_currentHole != null)
                    {
                        if(_block.GetMatchedHole() != _currentHole)
                        {

                        }
                    }
                }
            }
        }*/

        if(_currentPhaseNumber == 0 && _currentBlocksAndHoles.GetHoles()[0].GetObjectPlaced())
        {
            Debug.Log("In the building game, we are entering phase 2.");

            _currentHole = _currentBlocksAndHoles.GetHoles()[0];

            _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.0f);

            _currentHole = _currentBlocksAndHoles.GetHoles()[1];

            _currentHole.gameObject.SetActive(true);

            _currentPhaseNumber = 1;

            _instancedArrowAnimations[0].SetAnimateBoolean(false);

            if(_animationCoroutineArrow != null)
            {
                StopCoroutine(_animationCoroutineArrow);
            }

            _instancedArrowAnimations[0].GetArrowContainer().SetActive(false);

            _instancedArrowAnimations[0].GetNumber3DObject().SetActive(false);

            _instancedArrowAnimations[1].GetArrowContainer().SetActive(true);

            _instancedArrowAnimations[1].GetNumber3DObject().SetActive(true);

            _currentInstancedArrowAnimation = _instancedArrowAnimations[1];

            _animationCoroutineArrow = StartCoroutine(_instancedArrowAnimations[1].Animate());

            _gameProperties.SetResponseText("Well Done!\n\n2. Now, find the medicine, and drag it on the shaded area above the neck.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));

            if(_dialogues != null)
            {
                _dialogues.PlayClip("Dr. Salem Assembly Game Step 2");
            }

            _gameProperties.SignalToUpdateUI();
        }

        if(_currentPhaseNumber == 1 && _currentBlocksAndHoles.GetHoles()[1].GetObjectPlaced())
        {
            _currentHole = _currentBlocksAndHoles.GetHoles()[1];

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

            _gameProperties.SignalToUpdateUI();

            if (_dialogues != null)
            {
                _dialogues.PlayClip("Dr. Salem Assembly Game Step 3");
            }

            _gameProperties.SetResponseText("You're doing great!\n\n3. Remove the cap from the neck.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));


            if (_arrowAnimations[1].GetArrowContainer().activeSelf)
            {
                _arrowAnimations[1].GetArrowContainer().SetActive(false);
            }
        }

        if(_currentPhaseNumber == 2 && _cap != null)
        {
            if (_cap.GetComponent<DraggableClass>() != null)
            {
                if (_cap.GetComponent<DraggableClass>().GetTouchPhase() == TouchPhase.Ended && _cap.transform.localPosition.z >= 0.035f)
                {
                    _currentHole = _currentBlocksAndHoles.GetHoles()[2];

                    _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.7f);

                    _currentHole.gameObject.SetActive(true);

                    _currentPhaseNumber = 3;

                    _highlightingAnimationProperties.SetAnimateBoolean(false);

                    if(_animationCoroutineHighlight != null)
                    {
                        StopCoroutine(_animationCoroutineHighlight);
                    }

                    _cap.layer = 0;

                    Destroy(_cap);

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

                    _gameProperties.GetMeter().AddToValue(1);

                    _gameProperties.SetResponseText("We're almost done...\n\n4. Find the spacer and place it on the shaded area pointed by the arrow.", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));

                    if (_correctAudioClip != null)
                    {
                        AudioSource _as = _currentBlocksAndHoles.GetHoles()[0].GetAudioSource();

                        if(_as != null)
                        {
                            _as.clip = _correctAudioClip;

                            _as.Play();
                        }
                    }

                    _gameProperties.SignalToUpdateUI();
                }
                else if(_cap.GetComponent<DraggableClass>().GetTouchPhase() == TouchPhase.Ended)
                {
                    _cap.transform.localPosition = _capLocalPosition;

                    if (_incorrectAudioClip != null)
                    {
                        AudioSource _as = _currentBlocksAndHoles.GetHoles()[0].GetAudioSource();

                        if (_as != null)
                        {
                            _as.clip = _incorrectAudioClip;

                            _as.Play();
                        }
                    }
                }
            }
        }

        if(_currentPhaseNumber == 3 && _currentBlocksAndHoles.GetHoles()[2].GetObjectPlaced())
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

            StartCoroutine(FinalStep());

            _currentPhaseNumber = -1;

            _currentInstancedArrowAnimation = null;
        }
    }

    public override void StopGame()
    {
        base.StopGame();

        if(_currentInstancedArrowAnimation != null)
        {
            _currentInstancedArrowAnimation.SetAnimateBoolean(false);

            _currentInstancedArrowAnimation = null;
        }

        _currentPhaseNumber = 0;

        _floor.SetActive(false);

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

        _currentBlocksAndHoles.ClearLists();

        if (_currentGame == this)
        {
            _currentGame = null;
        }
    }

    public override void QuitGame()
    {
        if (_currentInstancedArrowAnimation != null)
        {
            _gameProperties.GetYesOrNoCanvas().GetNoButton().onClick.AddListener(delegate { _currentInstancedArrowAnimation.SetPauseAnimation(false); });

            _gameProperties.GetYesOrNoCanvas().GetNoButton().onClick.AddListener(delegate { _gameProperties.GetInformationCanvas().gameObject.SetActive(true); });

            _currentInstancedArrowAnimation.SetPauseAnimation(true);
        }

        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.AddListener(delegate { StopGame(); });

        _gameProperties.GetInformationCanvas().gameObject.SetActive(false);

        base.QuitGame();
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

        _cap.layer = 7;

        _animationCoroutineHighlight = StartCoroutine(_highlightingAnimationProperties.Animate());
    }

    IEnumerator FinalStep()
    {
        _gameProperties.SetResponseText("Excellent! We now have a complete inhaler!", new Color(0.0f, 1.0f, 0.0f, 1.0f), new Color(0.0f, 0.5f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));

        yield return new WaitForSeconds(5.0f);

        base.WinGame();

        _currentPhaseNumber = 0;
    }
}
