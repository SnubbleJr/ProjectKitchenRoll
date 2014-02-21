
#pragma strict

var target : Transform;
var xOffset = 0.0;
var yOffset = 0.0;
var smoothTime = 0.3;
private var thisTransform : Transform;
private var velocity : Vector2;

function Start()
{
	thisTransform = transform;
}

function Update() 
{
	thisTransform.position.x = Mathf.SmoothDamp( thisTransform.position.x, 
		(target.position.x + xOffset), velocity.x, smoothTime);
	thisTransform.position.y = Mathf.SmoothDamp( thisTransform.position.y, 
		(target.position.y + yOffset), velocity.y, smoothTime);
}