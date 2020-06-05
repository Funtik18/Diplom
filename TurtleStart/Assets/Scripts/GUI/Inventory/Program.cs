using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour{

	public ReferenceContainer head;
	public TMPro.TextMeshProUGUI label;
	public Button clearBtn;
	public Button closeBtn;
	public Container body;

	private void OnEnable() {
		if(clearBtn)
		clearBtn.onClick.AddListener(delegate {
			body.DisposeAll();
		});
		if(closeBtn)
		closeBtn.onClick.AddListener(delegate {
			InventoryOverseer._instance.DeleteContainer(head);
			InventoryOverseer._instance.DeleteContainer(body);

			Disable(false);
			body.DisposeAll();
			
			//OpenCloseFunction._instance.DeleteDisabledFunctions();
		});
	}

	public void Disable(bool _triger) {
		body.gameObject.SetActive(_triger);
		gameObject.SetActive(_triger);
	}
}
