using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour{

	public GameObject head;
	public TMPro.TextMeshProUGUI label;
	public Button clearBtn;
	public Button closeBtn;
	public Container body;

	private void OnEnable() {
		clearBtn.onClick.AddListener(delegate {
			body.DisposeAll();
		});

		closeBtn.onClick.AddListener(delegate {
			body.DisposeAll();
			Disable(false);
			//OpenCloseFunction._instance.DeleteDisableFunctions();
		});
	}

	public void Disable(bool _triger) {
		gameObject.SetActive(_triger);
	}
}
