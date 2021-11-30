using System.Collections.Generic;
using UnityEngine;

public class TurnOrderManager : MonoBehaviour
{
	public List<FramesUI> framesList = new List<FramesUI>();

 	private void ChangeOrder()
	{
		int positionToChange = UnityEngine.Random.Range(0, framesList.Count-1);
		List<FramesUI> tempFramesList = new List<FramesUI>();
		for (int i = positionToChange; i < framesList.Count; i++)
		{
			tempFramesList.Add(framesList[i]);
		}

		FramesUI frameDataTemp = tempFramesList[0];
		tempFramesList.RemoveAt(0);
		tempFramesList.Add(frameDataTemp);

		for (int i = positionToChange; i < framesList.Count; i++)
		{
			//Tengo problemas para setear los valores nuevos, y que se cambie el orden de la lista.
			var tempFrame = tempFramesList[0];
			var changedFrame = framesList[i]; 
			framesList[i] = tempFrame;
			//changedFrame.ChangeData(tempFrame.mechaImage, tempFrame.leftGunIcon, tempFrame.rightGunIcon, tempFrame.mechaName);
			changedFrame.ChangeData(tempFrame.mechaImage, tempFrame.mechaName);
			//framesList[i].ChangeName(tempFrame.mechaName);
			tempFramesList.RemoveAt(0);
		}
	}
}
