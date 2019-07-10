function EnvGui::ToggleInfiniteWater()
{
	if($EnvGui::InfiniteWater)
	{
		$EnvGui::InfiniteWater = 0;
		WaterPlane.setTransform($EnvGui::WaterPositionOld);
		WaterZone.setScale($EnvGui::WaterScaleOld);
	}
	else
	{
		$EnvGui::InfiniteWater = 1;
		$EnvGui::WaterPositionOld = WaterPlane.position;
		$EnvGui::WaterScaleOld = WaterZone.scale;
		
		WaterPlane.setTransform("0 0 100000");
		WaterZone.setScale("1e+006 1e+006 100000");
	}
	EnvGui::updateVar(EnvGui, WaterHeight);
}

//%btn = new GuiBitmapButtonCtrl(EnvGuiTogGround)
//{
//    profile = BlockButtonProfile;
//    horizSizing = "left";
//    vertSizing = "top";
//    position = "114 397";
//    extent = "96 38";
//    command = "EnvGui.updateGroundEnabled();";
//    text = "Toggle Ground";
//    bitmap = "base/client/ui/button2";
//    mcolor = "255 0 0 255";
//};
//EnvGui_Window.add(%btn);