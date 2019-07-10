//if these don't exist, pre-set them to their defaults
if(!$jumpVelocity1)
{
	$jumpVelocity1 = 8;
	$jumpVelocity2 = 12;
}

if(!$jumpTimeout1)
{
	$jumpTimeout1 = 500;
	$jumpTimeout2 = 750;
}

if(!$jumpSmoothing)
{
	$jumpSmoothing = 1;
}

$JumpControl::Version = 2;

exec("./JumpControl.cs");