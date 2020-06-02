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
			body.DisposeAll();
			Disable(false);

			InventoryOverseer._instance.DeleteContainer(head);
			InventoryOverseer._instance.DeleteContainer(body);

			OpenCloseFunction._instance.DeleteDisabledFunctions();
		});
	}

	public void Disable(bool _triger) {
		gameObject.SetActive(_triger);
	}
}
