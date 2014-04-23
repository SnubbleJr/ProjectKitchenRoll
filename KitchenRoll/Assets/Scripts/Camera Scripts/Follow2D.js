
#pragma strict

var target : Transform;
var xOffset = 0.0;
var yOffset = 0.0;
private var thisTransform : Transform;

function Start()
{
	thisTransform = transform;
}

function Update() 
{
	thisTransform.position.x = (target.position.x + xOffset);
	thisTransform.position.y = (target.position.y + yOffset);
}