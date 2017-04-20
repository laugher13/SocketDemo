using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol.Dto;

public class Login : MonoBehaviour {

    private InputField inputUserName;
    private InputField inputPassword;
    private Button btnLogin;

	void Start () {
        inputUserName = this.transform.FindChild("InputUserName").GetComponent<InputField>();
        inputPassword = this.transform.FindChild("InputPassWord").GetComponent<InputField>();
        btnLogin = this.transform.FindChild("BtnLogin").GetComponent<Button>();
        btnLogin.onClick.AddListener(OnLoginClick);
	}
	
	
	void Update () {
	    
	}

    private void OnLoginClick()
    {
        print("login");
        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = inputUserName.text;
        dto.password = inputPassword.text;
        NetIO.Instance.write(Protocol.AreaProtocol.TYPE_LOGIN, 0, Protocol.LoginProtocol.LOGIN_CREQ, dto);
    }
}
