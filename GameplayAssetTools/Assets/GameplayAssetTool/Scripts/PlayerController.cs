using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class PlayerController : MonoBehaviour {
	private Animator playerAC; //player Animator
	private Rigidbody playerRB; //Player Rigidbody

	private bool airborne; //airborne state
	private bool crouch; //crouch state
	private bool fastCrouch; //crouch fast movement
	private bool isCrouchToIdle=true;//if crouch to idle state is possible at any given state

	private float crouchDirection; //angle of crouch orientation movement
	private float plCameraAngle; //Angle between player and Camera.

	private int timesPressedC=0;
	private int layerHierarchy;

	private GameObject playerCamera; //main camera
	private LineRenderer playerLine; //height Check line for crouch to idle stand.

	[Tooltip("Use the (PlayerController.cs) only when having a Humanoid Rig Type with the associated AC provided."+"\n"+"The Components Animator,Rigidbody and Collider are required.")]
	public string README;

	void Start () {
		playerRB = GetComponent<Rigidbody> ();
		playerAC = GetComponent<Animator> ();
		airborne = false;
		layerHierarchy=playerAC.GetLayerIndex("Crouch");
		crouchDirection = 0f;
		playerCamera = GameObject.FindGameObjectWithTag ("DepthTest");
		playerLine = playerRB.gameObject.GetComponent<LineRenderer> ();
	}
	void LateUpdate(){
		if (airborne == true && playerAC.applyRootMotion==true) {
			StartCoroutine (checkAirborne ());
			airborne = false;
		}
	}
	void FixedUpdate(){
		//smooth transition from fast crouch movement to normal walk/run cycle.
		if (crouch==false)
			StartCoroutine (resetAnimatorSpeed());

		//front movement
		if (Input.GetKey(KeyCode.W) && crouch==false) {
			playerAC.SetBool("isIdle",false);
			playerAC.SetBool("turnLeftIdle",false);
			playerAC.SetBool("turnRightIdle",false);
			playerAC.SetBool("isRunning",false);
			playerAC.SetBool("isWalking",true);
			playerRB.AddForce(Vector3.forward*1f*Time.deltaTime);
			if(Input.GetKey(KeyCode.A) && airborne==false){ //active walk left
				playerAC.SetBool("isWalkingLeft",true);
			}
			if(!Input.GetKey(KeyCode.A)){//inactive walk left
				playerAC.SetBool("isWalkingLeft",false);
			}
			if(Input.GetKey(KeyCode.D) && airborne==false){ //active walk right
				playerAC.SetBool("isWalkingRight",true);
			}
			if(!Input.GetKey(KeyCode.D)){//inactive walk right
				playerAC.SetBool("isWalkingRight",false);
			}
		}

		//front sprint movement 
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && crouch==false){
			playerAC.SetBool("isIdle",false);
			playerAC.SetBool("turnLeftIdle",false);
			playerAC.SetBool("turnRightIdle",false);
			playerAC.SetBool("isWalking",false);
			playerAC.SetBool("isRunning",true);
			playerRB.AddForce(Vector3.forward*2f*Time.deltaTime);
			if(Input.GetKey(KeyCode.A) && airborne==false){//active run left
				playerAC.SetBool("isRunningLeft",true);
			}
			if(!Input.GetKey(KeyCode.A)){//inactive run left
				playerAC.SetBool("isRunningLeft",false);
			}
			if(Input.GetKey(KeyCode.D) && airborne==false){ //active run right
				playerAC.SetBool("isRunningRight",true);
			}
			if(!Input.GetKey(KeyCode.D)){//inactive run right
				playerAC.SetBool("isRunningRight",false);
			}
		}

		//idle position
		if (!Input.GetKey(KeyCode.W) && crouch==false) {
			playerAC.SetBool("isIdle",true);
			playerAC.SetBool("isRunning",false);
			playerAC.SetBool("isWalking",false);
			playerAC.SetBool("isRunningRight",false);
			playerAC.SetBool("isRunningLeft",false);
			playerAC.SetBool("isWalkingRight",false);
			playerAC.SetBool("isWalkingLeft",false);
			playerRB.AddForce(Vector3.zero*Time.deltaTime);

			if(playerAC.GetBool("isIdle") && airborne==false){//idle check
				playerAC.SetBool("turnLeftIdle",false);
				playerAC.SetBool("turnRightIdle",false);
			}

			if(Input.GetKey(KeyCode.A) && airborne==false){//idle left turn
				playerAC.SetBool("turnRightIdle",false);
				playerAC.SetBool("turnLeftIdle",true);
			}
			if(Input.GetKey(KeyCode.D) && airborne==false){//idle right turn
				playerAC.SetBool("turnLeftIdle",false);
				playerAC.SetBool("turnRightIdle",true);
			}
			if(Input.GetKey(KeyCode.S) && airborne==false){
				bool isKeyPressed=false;
				playerAC.SetBool("isTurning",true);
				isKeyPressed=true;
				if(plCameraAngle>=0f && isKeyPressed==true){ //turn back left
					playerAC.SetFloat("turningAngle",180f-plCameraAngle);
				}
				if(plCameraAngle<0f && isKeyPressed==true){ //turn back right
					playerAC.SetFloat("turningAngle",(plCameraAngle*-1f)-180f);
				}
			}
			if(!Input.GetKey(KeyCode.S) && airborne==false){
				playerAC.SetBool("isTurning",false);
			}
		}

		//jump up
		if(Input.GetKeyDown(KeyCode.Space) && airborne==false && crouch==false){

			if(!playerAC.GetBool("Airborne") && playerAC.GetBool("isIdle")){ //idle jump up
				if (!playerAC.GetBool ("turnLeftIdle") && !playerAC.GetBool ("turnRightIdle")) {
					playerRB.AddExplosionForce (5f, new Vector3 (playerRB.gameObject.transform.position.x, playerRB.gameObject.transform.position.y + Vector3.up.y, playerRB.gameObject.transform.position.z), 2.5f, 1f, ForceMode.Impulse);
					playerAC.applyRootMotion = false;
					playerAC.SetBool ("Airborne", true);
				}
			}
			if(!playerAC.GetBool("AirborneWalk") && playerAC.GetBool("isWalking")){ //walk jump up
				if(!playerAC.GetBool("isWalkingLeft") && !playerAC.GetBool("isWalkingRight")){
					playerRB.AddExplosionForce(5f,new Vector3(playerRB.gameObject.transform.position.x,playerRB.gameObject.transform.position.y+Vector3.up.y,playerRB.gameObject.transform.position.z),2.5f,1f,ForceMode.Impulse);
					playerAC.applyRootMotion=false;
					playerAC.SetBool("AirborneWalk",true);
				}
			}
			if(!playerAC.GetBool("AirborneRun") && playerAC.GetBool("isRunning")){ //run jump up
				if (!playerAC.GetBool ("isRunningLeft") && !playerAC.GetBool ("isRunningRight")) {
					playerRB.AddExplosionForce (5f, new Vector3 (playerRB.gameObject.transform.position.x, playerRB.gameObject.transform.position.y + Vector3.up.y, playerRB.gameObject.transform.position.z), 2.5f, 1f, ForceMode.Impulse);
					playerAC.applyRootMotion = false;
					playerAC.SetBool ("AirborneRun", true);
				}
			}
		}

		//jump up to idle
		if (!Input.GetKeyDown (KeyCode.Space) && playerRB.useGravity==false) {
			if(!playerAC.IsInTransition(0) && airborne==false){
				if(playerAC.GetBool("isIdle")){
					if(playerAC.GetBool("Airborne")){
						playerAC.SetBool("Airborne",false);
						playerAC.applyRootMotion=true;
					}
				}
				if(!playerAC.GetBool("isWalking")){ //if not walking action while airborne
					if(playerAC.GetBool("AirborneWalk")){
						playerAC.SetBool("AirborneWalk",false);
						playerAC.applyRootMotion=true;
					}
				}
				if(playerAC.GetBool("isWalking")){ //if  walking action while airborne
					if(playerAC.GetBool("AirborneWalk")){
						playerAC.SetBool("AirborneWalk",false);
						playerAC.applyRootMotion=true;
					}
				}
				if(!playerAC.GetBool("isRunning")){ //if not running action while airborne
					if(playerAC.GetBool("AirborneRun")){
						playerAC.SetBool("AirborneRun",false);
						playerAC.applyRootMotion=true;
					}
				}
				if(playerAC.GetBool("isRunning")){ //if running action while airborne
					if(playerAC.GetBool("AirborneRun")){
						playerAC.SetBool("AirborneRun",false);
						playerAC.applyRootMotion=true;
					}
				}
			}
			//Smooth transition from idle airborn to walk or run states
			if (playerAC.GetBool ("Airborne") && (playerAC.GetBool ("isWalking") || playerAC.GetBool ("isRunning"))) {
				playerAC.SetBool("Airborne",false);
				playerAC.applyRootMotion=true;
			}
		}

		if (Input.GetMouseButton (2)) { //Align player's orientation to camera's angle
			if(plCameraAngle > 10f){
				if (playerAC.GetBool ("isIdle")) {
					playerAC.SetBool ("turnRightIdle", true);
				}
				if(playerAC.GetBool("isWalking")){
					playerAC.SetBool ("isWalkingRight", true);
				}
				if (playerAC.GetBool ("isRunning")) {
					playerAC.SetBool ("isRunningRight", true);
				}
			}
			if (plCameraAngle <-10f) {
				if (playerAC.GetBool ("isIdle")) {
					playerAC.SetBool ("turnLeftIdle", true);
				}
				if(playerAC.GetBool("isWalking")){
					playerAC.SetBool ("isWalkingLeft", true);
				}
				if (playerAC.GetBool ("isRunning")) {
					playerAC.SetBool ("isRunningLeft", true);
				}
			}
		}

		//crouch Idle to crouch strafe Left
		if (Input.GetKey (KeyCode.A) && crouch == true && playerAC.GetBool ("isCrouchWalking") == false) {
			playerAC.SetBool ("isCrouchTurnLeftIdle", true);
		}

		//crouch strafe Left to crouch Idle
		if (!Input.GetKey (KeyCode.A) && crouch == true) {
			playerAC.SetBool ("isCrouchTurnLeftIdle", false);
			playerRB.AddForce(Vector3.zero*Time.deltaTime);
		}

		//crouch Idle to strafe Right
		if (Input.GetKey (KeyCode.D) && crouch == true && playerAC.GetBool("isCrouchWalking")==false) {
			playerAC.SetBool("isCrouchTurnRightIdle",true);
		}

		//crouch strafe Right to crouch Idle
		if (!Input.GetKey (KeyCode.D) && crouch == true) {
			playerAC.SetBool ("isCrouchTurnRightIdle", false);
			playerRB.AddForce(Vector3.zero*Time.deltaTime);
		}

		//crouch idle to crouch walk
		if(Input.GetKey(KeyCode.W) && crouch==true){
			playerAC.SetBool("isCrouchWalking",true);
			playerRB.AddForce(Vector3.forward*2f*Time.deltaTime);
			//crouch fast movement 
			if(Input.GetKey (KeyCode.LeftShift)){
				playerAC.speed=Mathf.Lerp(playerAC.speed,2f,Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.A)){ //strafe left while crouch moving
				crouchDirection-=0.01f;
				if(crouchDirection>=-0.5f){
					playerAC.SetFloat("angleRot",crouchDirection);//set crouch direction
				}
			}
			if(Input.GetKey(KeyCode.D)){//strafe right crouch moving
				crouchDirection+=0.01f;
				if(crouchDirection<=0.5f){
					playerAC.SetFloat("angleRot",crouchDirection);
				}
			}
			if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
				crouchDirection=Mathf.Lerp(crouchDirection,0,Time.deltaTime);
				playerAC.SetFloat("angleRot",crouchDirection);
			}
			if(!Input.GetKey (KeyCode.LeftShift)){
				StartCoroutine(resetAnimatorSpeed()); //reset speed to normal
			}
		}

		//crouch walk to crouch idle
		if(!Input.GetKey(KeyCode.W) && crouch==true){
			playerAC.SetBool("isCrouchWalking",false);
			playerRB.AddForce(Vector3.zero*Time.deltaTime);
			StartCoroutine(resetAnimatorSpeed()); //reset speed to normal
		}
	}

	void Update(){
		plCameraAngle = Mathf.DeltaAngle (this.gameObject.transform.rotation.eulerAngles.y, playerCamera.transform.rotation.eulerAngles.y); //angle between player & camera

		playerLine.SetPosition(0,new Vector3(playerRB.transform.position.x,playerRB.transform.position.y+1f,playerRB.transform.position.z)); //set start point of line
		playerLine.SetPosition(1,GameObject.FindGameObjectWithTag("Anchor").transform.position); //set end point of line
		RaycastHit hit; //temp variable of data gathering ray.
	 
		if (Physics.Linecast (new Vector3 (playerRB.transform.position.x, playerRB.transform.position.y + 1f, playerRB.transform.position.z), GameObject.FindGameObjectWithTag ("Anchor").transform.position, out hit)) {
			if (hit.distance < (playerRB.gameObject.GetComponent<CapsuleCollider> ().center.y)) {
				isCrouchToIdle = false;
			}
		}
		else {
			isCrouchToIdle = true;
		}
			
		//crouch
		if (Input.GetKeyDown (KeyCode.C) && airborne == false) {
			if(timesPressedC%2==0){
				float source = 1f;
				float target = 0f;
				float interPolator = 0.0f;
				crouch=true;
				playerAC.SetBool("isIdle",false);
				StartCoroutine(smoothCrouch(source,target,interPolator));
				playerRB.gameObject.GetComponent<CapsuleCollider>().center= new Vector3(0f,0.5f,0f);
				playerRB.gameObject.GetComponent<CapsuleCollider>().height=1f;
			}
			if (timesPressedC % 2 == 1) {
				if (isCrouchToIdle == true) {
					float source = 0f;
					float target = 1f;
					float interPolator = 0.0f;
					crouch = false;
					playerAC.SetBool ("isIdle", true);
					playerAC.SetBool ("isCrouchWalking", false);
					playerAC.SetBool ("isCrouchTurnLeftIdle", false);
					playerAC.SetBool ("isCrouchTurnRightIdle", false);
					StartCoroutine (smoothCrouch (source, target, interPolator));
					playerRB.gameObject.GetComponent<CapsuleCollider> ().center = new Vector3 (0f, 1f, 0f);
					playerRB.gameObject.GetComponent<CapsuleCollider> ().height = 2f;
				}
			}
			if(timesPressedC>=99){
				timesPressedC=0;
			}else{
				timesPressedC+=1;
			}
		}
	}

	void OnCollisionEnter(Collision col){
		airborne = false;
		playerRB.useGravity = false;
	}
	void OnCollisionExit(Collision col){
		airborne = true;
		playerRB.useGravity = true;
	}

	//Animator speed smooth reset
	IEnumerator resetAnimatorSpeed(){
		if(playerAC.speed>1.0000f){
			playerAC.speed=Mathf.Lerp(playerAC.speed,1f,Time.deltaTime);
		}
		yield return new WaitForSeconds (1f);
	}

	//Airborne Check
	IEnumerator checkAirborne(){
		if(airborne==true)
			yield return new WaitForSeconds(11);
		airborne = false;
	}

	//Smooth Crouch Controller
	IEnumerator smoothCrouch(float source,float target,float t){
		yield return new WaitForEndOfFrame();
		playerAC.SetLayerWeight (layerHierarchy,Mathf.Lerp(source, target, t));
	}
}
