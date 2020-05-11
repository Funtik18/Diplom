using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaItem : BasicItem{

	public override Item Item {
		get {
			return this.item;
		}
		set {
			this.item = value;
			if(this.item == null) return;
		}
	}

	public override Image CurrentImage {
		get {
			return null;
		}
	}

	public override bool IsEmpty() {
		return this.item;
	}
	public override void Dispose() {
		this.item = null;
	}

}
