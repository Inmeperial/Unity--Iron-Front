using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FramesUI : MonoBehaviour
{
	public Image mechaImage;
	public Image leftGunIcon;
	public Image rightGunIcon;
	public TextMeshProUGUI mechaName;
	public CustomButton selectionButton;
	
	FramesUI(Image mecha, Image leftGun, Image rightGun, TextMeshProUGUI myName)
	{
		mechaImage = mecha;
		leftGunIcon = leftGun;
		rightGunIcon = rightGun;
		mechaName = myName;
	}

	public void ChangeData(Image newMechaImage, Image newMechaLeftIcon, Image newMechaRightIcon, TextMeshProUGUI newMechaName)
	{
		mechaImage = newMechaImage;
		leftGunIcon = newMechaLeftIcon;
		rightGunIcon = newMechaRightIcon;
		mechaName.text = newMechaName.text;
	}

	public void ChangeName(TextMeshProUGUI newName)
	{
		mechaName.text = newName.text;
	}
	/*public FramesUI ChangeData(FramesUI newFrame)
	{
		return new FramesUI(newFrame.mechaImage, newFrame.leftGunIcon, newFrame.rightGunIcon, newFrame.mechaName);
	}*/
}
