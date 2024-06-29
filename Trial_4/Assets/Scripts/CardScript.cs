using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField]
    protected bool _cardFlipped;

    [SerializeField]
    protected string _cardName;

    [SerializeField]
    protected bool _cardDone;

    [SerializeField]
    protected bool _isInfoCard;

    [SerializeField]
    protected string _cardDescription;

    [SerializeField]
    CardGroupScript _group;

    PlayerController _controller;

    Vector3 _originalPosition = Vector3.zero;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    MeshRenderer _renderer;

    [SerializeField]
    public Text _text;

    int _cardNumber = -1;

    // Start is called before the first frame update
    void Start()
    {
        if(_controller == null)
        {
            _controller = new PlayerController();

            _controller.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouch();

        CheckCardStatus();
    }

    private void OnEnable()
    {
        if(_controller == null)
        {
            _controller = new PlayerController();

            _controller.Enable();
        }
    }

    public bool GetCardFlipped()
    {
        return _cardFlipped;
    }

    public string GetCardName()
    {
        return _cardName;
    }

    public bool GetCardDone()
    {
        return _cardDone;
    }

    public bool GetIsInfoCard()
    {
        return _isInfoCard;
    }

    public CardGroupScript GetGroup()
    {
        return _group;
    }

    public Vector3 GetOriginalPosition()
    {
        return _originalPosition;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public MeshRenderer GetRenderer()
    {
        return _renderer;
    }

    public int GetCardNumber()
    {
        return _cardNumber;
    }

    public string GetCardDescription()
    {
        return _cardDescription;
    }

    public void SetCardFlipped(bool _input)
    {
        _cardFlipped = _input;
    }

    public void SetCardDone(bool _input)
    {
        _cardDone = _input;
    }

    public void SetIsInfoCard(bool _input)
    {
        _isInfoCard = _input;
    }

    public void SetCardName(string _input)
    {
        _cardName = _input;
    }

    public void SetGroup(CardGroupScript _input)
    {
        _group = _input;
    }

    public void SetOriginalPosition(Vector3 _input)
    {
        _originalPosition = _input;
    }

    public void SetCamera(Camera _input)
    {
        _camera = _input;
    }

    public void SetCardNumber(int _input)
    {
        _cardNumber = _input;
    }

    public void SetCardDescription(string _input)
    {
        _cardDescription = _input;
    }

    void CheckTouch()
    {
        if(_controller == null || _camera == null || _group == null)
        {
            /*if (_text != null)
            {
                if (_text.text == "")
                {
                    _text.text = "Something is missing...";
                    _text.color = Color.red;
                    _text.gameObject.GetComponent<Outline>().effectColor = new Color(0.5f, 0.0f, 0.0f);
                }
            }*/
            return;
        }

        if(_controller.Touch.TouchPress.WasPressedThisFrame())
        {
            Vector2 _position = _controller.Touch.Position.ReadValue<Vector2>();

            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                _position = _controller.Move.CursorPosition.ReadValue<Vector2>();
            }

            Ray _ray = _camera.ScreenPointToRay(_position);

            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit))
            {
                if(_hit.collider.transform == transform)
                {
                    _group.SetSelectedCard(this);
                }
            }
        }
    }

    void CheckCardStatus()
    {
        if(_cardDone)
        {
            gameObject.SetActive(false);
        }
        else if(_cardFlipped)
        {
            transform.localRotation = Quaternion.Euler(90.0f, 90.0f, 0.0f);

            SetBlackOpacity(0.0f);
        }
        else
        {
            SetBlackOpacity(1.0f);

            if (transform.localRotation != Quaternion.Euler(-90.0f, 90.0f, 0.0f))
            {
                transform.localRotation = Quaternion.Euler(-90.0f, 90.0f, 0.0f);
            }
        }
    }

    void SetBlackOpacity(float _input)
    {
        if(_renderer.materials.Length == 0)
        {
            return;
        }

        if (_renderer.materials[0] == null)
        {
            return;
        }

        const string _referenceName = "_Black_Opacity_Float";

        if (_renderer.material.GetFloat(_referenceName) != _input)
        {
            _renderer.materials[0].SetFloat(_referenceName, _input);
        }
    }
}
