#if PUN_2_OR_NEWER
using Photon.Pun;
using Photon.Realtime;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class NetworkPlayer :
#if PUN_2_OR_NEWER
MonoBehaviourPunCallbacks, IPunObservable, IPunOwnershipCallbacks 
#else
        MonoBehaviour
#endif
        {
        #region Global Variables
        [Header("Remote player's glove selection")]
        [Tooltip("Remote Player's Gloves, must fill in from prefab")]
        [SerializeField] GameObject greenGlovesL;
        [SerializeField] GameObject caucHandsL;
        [SerializeField] GameObject darkHandsL;
        [SerializeField] GameObject lightHalfGlovesL;
        [SerializeField] GameObject tanHalfGlovesL;
        [SerializeField] GameObject greenGlovesR;
        [SerializeField] GameObject caucHandsR;
        [SerializeField] GameObject darkHandsR;
        [SerializeField] GameObject lightHalfGlovesR;
        [SerializeField] GameObject tanHalfGlovesR;

        [Header("Remote Player's Jammer attachment selections")]
        [Tooltip("Remote Player's Attachments, must fill in from prefab")]
        public GameObject jammerForks;
        public GameObject jammerr2D2;

        [Header("Local Player Variables")]
        [Tooltip("Transform of the local player's head to track. This will be applied to the Remote Player's Head Transform")]
        public Transform PlayerHeadTransform;
        [Tooltip("Transform of the local player's LHand to track. This will be applied to the Remote Player's LHand Transform")]
        public Transform PlayerLeftHandTransform;
        [Tooltip("Transform of the local player's RHand to track. This will be applied to the Remote Player's RHand Transform")]
        public Transform PlayerRightHandTransform;
        [Tooltip("Transform of the local player's Jammer to track. This will be applied to the Remote Player's Jammer Transform")]
        public Transform PlayerJammerTransform;


        [Header("Remote Player Variables")]
        [Tooltip("Transform of the remote player's head. This will be updated during Update")]
        public Transform RemoteHeadTransform;
        [Tooltip("Transform of the remote player's Jammer. This will be updated during Update")]
        public Transform RemoteJammerTransform;
        [Tooltip("Transform of the remote player's left hand / controller. This will be updated during Update")]
        public Transform RemoteLeftHandTransform;
        [Tooltip("Transform of the remote player's right hand / controller. This will be updated during Update")]
        public Transform RemoteRightHandTransform;


        // Store positions to move between updates
        private Vector3 _syncHeadStartPosition = Vector3.zero;
        private Vector3 _syncHeadEndPosition = Vector3.zero;
        private Quaternion _syncHeadStartRotation = Quaternion.identity;
        private Quaternion _syncHeadEndRotation = Quaternion.identity;
        private Vector3 _syncJammerStartPos = Vector3.zero;
        private Vector3 _syncJammerEndPos = Vector3.zero;
        private Quaternion _syncJammerStartRot = Quaternion.identity;
        private Quaternion _syncJammerEndRot = Quaternion.identity;

        // Store positions to move between updates
        private Vector3 _syncLHandStartPosition = Vector3.zero;
        private Vector3 _syncLHandEndPosition = Vector3.zero;
        private Quaternion _syncLHandStartRotation = Quaternion.identity;
        private Quaternion _syncLHandEndRotation = Quaternion.identity;

        // Store positions to move between updates
        private Vector3 _syncRHandStartPosition = Vector3.zero;
        private Vector3 _syncRHandEndPosition = Vector3.zero;
        private Quaternion _syncRHandStartRotation = Quaternion.identity;
        private Quaternion _syncRHandEndRotation = Quaternion.identity;

        [Header("Hand Animation Variables")]
        // Send Hand Animation info to others
        //public HandController LeftHandController;
        //public HandController RightHandController;
        [Tooltip("How fast to animate the fingers on the remote players hands")]
        public float HandAnimationSpeed = 20f;
        // Receive Animation info
        public Animator RemoteLeftHandAnimator;
        public Animator RemoteRightHandAnimator;

        [Header("Grabber Variables")]
        [Tooltip("Local Player's Left Grabber. Used to determine which objects are nearby")]
       // public Grabber LeftGrabber;
       // GrabbablesInTrigger gitLeft;

        //[Tooltip("Local Player's Right Grabber. Used to determine which objects are nearby")]
        //public Grabber RightGrabber;
       // GrabbablesInTrigger gitRight;

        

        // Used for Hand Interpolation
        private float _syncLeftGripStart;
        private float _syncRightGripStart;
        private float _syncLeftPointStart;
        private float _syncRightPointStart;
        private float _syncLeftThumbStart;
        private float _syncRightThumbStart;

        private float _syncLeftGripEnd;
        private float _syncRightGripEnd;
        private float _syncLeftPointEnd;
        private float _syncRightPointEnd;
        private float _syncLeftThumbEnd;
        private float _syncRightThumbEnd;

        // Interpolation values
        private float _lastSynchronizationTime = 0f;
        private float _syncDelay = 0f;
        private float _syncTime = 0f;

        // network request grabbable permission
        protected double lastRequestTime = 0;
        protected float requestInterval = 0.1f; // 0.1 = 10 times per second
        Dictionary<int, double> requestedGrabbables;

        bool disabledObjects = false;

        private bool _syncLeftHoldingItem;
        private bool _syncRightHoldingItem;


        #endregion

       
#if PUN_2_OR_NEWER

        void Start() {
            //Grab refs to our Grabbables
           /* LeftGrabber = GameObject.Find("LeftController").GetComponentInChildren<Grabber>();
            gitLeft = LeftGrabber.GetComponent<GrabbablesInTrigger>();

            RightGrabber = GameObject.Find("RightController").GetComponentInChildren<Grabber>();
            gitRight = RightGrabber.GetComponent<GrabbablesInTrigger>();

            requestedGrabbables = new Dictionary<int, double>();

            //Set Remote players gloves and vic to match local player's selections
            if (photonView.IsMine)
            {
                photonView.RPC("ChangeAvatarGlovesPerSelections", RpcTarget.AllBuffered, UserGameCustomizer.instance.gloveSelect);
                photonView.RPC("ChangeAvatarVicPerSelections", RpcTarget.AllBuffered, UserGameCustomizer.instance.vicSelect);
            }*/
        }

        void Update() {
          
            // Check for request to grab object
            checkGrabbablesTransfer();
           
                
            // Remote Player
            if (!photonView.IsMine) {

              
                if (disabledObjects) {
                    toggleObjects(true);
                }

                // Keeps latency in mind to keep player in sync
                _syncTime += Time.deltaTime;
                float synchValue = _syncTime / _syncDelay;

                // Update Head and Hands Positions
                updateRemotePositionRotation(RemoteHeadTransform, _syncHeadStartPosition, _syncHeadEndPosition, _syncHeadStartRotation, _syncHeadEndRotation, synchValue);
                updateRemotePositionRotation(RemoteLeftHandTransform, _syncLHandStartPosition, _syncLHandEndPosition, _syncLHandStartRotation, _syncLHandEndRotation, synchValue);
                updateRemotePositionRotation(RemoteRightHandTransform, _syncRHandStartPosition, _syncRHandEndPosition, _syncRHandStartRotation, _syncRHandEndRotation, synchValue);
                updateRemotePositionRotation(RemoteJammerTransform, _syncJammerStartPos, _syncJammerEndPos, _syncJammerStartRot, _syncJammerEndRot, synchValue);
               
                // Update animation info
                if (RemoteLeftHandAnimator) {
                    _syncLeftGripStart = Mathf.Lerp(_syncLeftGripStart, _syncLeftGripEnd, Time.deltaTime * HandAnimationSpeed);
                    RemoteLeftHandAnimator.SetFloat("Flex", _syncLeftGripStart);
                    RemoteLeftHandAnimator.SetLayerWeight(0, 1);

                    _syncLeftPointStart = Mathf.Lerp(_syncLeftPointStart, _syncLeftPointEnd, Time.deltaTime * HandAnimationSpeed);
                    RemoteLeftHandAnimator.SetLayerWeight(2, _syncLeftPointStart);

                    _syncLeftThumbStart = Mathf.Lerp(_syncLeftThumbStart, _syncLeftThumbEnd, Time.deltaTime * HandAnimationSpeed);
                    RemoteLeftHandAnimator.SetLayerWeight(1, _syncLeftThumbStart);

                    // Default to grip if holding an item
                    if(_syncLeftHoldingItem) {
                        RemoteLeftHandAnimator.SetLayerWeight(0, 0);
                        RemoteLeftHandAnimator.SetFloat("Flex", 1);
                        RemoteLeftHandAnimator.SetFloat(1, 0);
                        RemoteLeftHandAnimator.SetFloat(2, 0);
                    }
                    else {
                        RemoteLeftHandAnimator.SetInteger("Pose", 0);
                    }
                }
                if (RemoteRightHandAnimator) {
                    _syncRightGripStart = Mathf.Lerp(_syncRightGripStart, _syncRightGripEnd, Time.deltaTime * HandAnimationSpeed);
                    RemoteRightHandAnimator.SetFloat("Flex", _syncRightGripStart);

                    _syncRightPointStart = Mathf.Lerp(_syncRightPointStart, _syncRightPointEnd, Time.deltaTime * HandAnimationSpeed);
                    RemoteRightHandAnimator.SetLayerWeight(2, _syncRightPointStart);

                    _syncRightThumbStart = Mathf.Lerp(_syncRightThumbStart, _syncRightThumbEnd, Time.deltaTime * HandAnimationSpeed);
                    RemoteRightHandAnimator.SetLayerWeight(1, _syncRightThumbStart);

                    // Default to grip if holding an item
                    if (_syncRightHoldingItem) {
                        RemoteRightHandAnimator.SetLayerWeight(0, 0);
                        RemoteRightHandAnimator.SetFloat("Flex", 1);
                        RemoteRightHandAnimator.SetLayerWeight(1, 0);
                        RemoteRightHandAnimator.SetLayerWeight(2, 0);
                    }
                    else {
                        RemoteRightHandAnimator.SetInteger("Pose", 0);
                    }
                }
            }
            else {
                if(!disabledObjects) {
                    toggleObjects(false);
                }
            }
          

        }
        
        /// <summary>
        /// Assigns objects from the local player to allow for remote player to sync actions 
        /// </summary>
        public void AssignPlayerObjects() {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            PlayerHeadTransform = getChildTransformByName(player.transform, "CenterEyeAnchor");
            PlayerJammerTransform = getChildTransformByName(player.transform, "LocalPlayerPrefab");

            // Using an explicit Transform name to make sure we grab the right one in the scene
            PlayerLeftHandTransform = GameObject.Find("ModelsLeft").transform;
            //LeftHandController = PlayerLeftHandTransform.parent.GetComponentInChildren<HandController>();
           
            PlayerRightHandTransform = GameObject.Find("ModelsRight").transform;
            //RightHandController = PlayerRightHandTransform.parent.GetComponentInChildren<HandController>();
           
            
        }

        /// <summary>
        /// Searches a GameObject's child transforms by name
        /// </summary>
        /// <param name="search"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Transform getChildTransformByName(Transform search, string name) {
            Transform[] children = search.GetComponentsInChildren<Transform>();
            for (int x = 0; x < children.Length; x++) {
                Transform child = children[x];
                if (child.name == name) {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Turns off local player's remote player on local player's machine
        /// </summary>
        /// <param name="enableObjects"></param>
        void toggleObjects(bool enableObjects) {
            RemoteHeadTransform.gameObject.SetActive(enableObjects);
            RemoteLeftHandTransform.gameObject.SetActive(enableObjects);
            RemoteRightHandTransform.gameObject.SetActive(enableObjects);
            RemoteJammerTransform.gameObject.SetActive(enableObjects);
            disabledObjects = !enableObjects;
        }
        
        /// <summary>
        /// Checks for grabbable transfer between users on netwrok
        /// </summary>
        void checkGrabbablesTransfer() {

            // Cap the request period
            if (PhotonNetwork.Time - lastRequestTime < requestInterval) {
                return;
            }           
           
            //requestOwnerShipForNearbyGrabbables(gitLeft);
            //requestOwnerShipForNearbyGrabbables(gitRight);
        }

        /// <summary>
        /// Requests ownership of object grabbable in remote grabbable
        /// </summary>
        /// <param name="grabbables"></param>
        /*void requestOwnerShipForNearbyGrabbables(GrabbablesInTrigger grabbables) {

            if(grabbables == null) {
                return;
            }

            // In Hand
            foreach (var grab in grabbables.NearbyGrabbables) {
                PhotonView view = grab.Value.GetComponent<PhotonView>();

                if (view != null && RecentlyRequested(view) == false && !view.AmOwner) {
                    RequestGrabbableOwnership(view);
                }
            }

            // Remote Grabbables
            foreach (var grab in grabbables.ValidRemoteGrabbables) {
                PhotonView view = grab.Value.GetComponent<PhotonView>();

                if (view != null && RecentlyRequested(view) == false && !view.AmOwner) {
                    RequestGrabbableOwnership(view);
                }
            }
        }*/

        /// <summary>
        /// Checks for which player requested
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public virtual bool RecentlyRequested(PhotonView view) {
            // Previously requested if in list and requested less than 3 seconds ago
            return requestedGrabbables != null && requestedGrabbables.ContainsKey(view.ViewID) && PhotonNetwork.Time - requestedGrabbables[view.ViewID] < 3f;
        }

        /// <summary>
        /// requests ownership of grabbable
        /// </summary>
        /// <param name="view"></param>
        public virtual void RequestGrabbableOwnership(PhotonView view) {

            lastRequestTime = PhotonNetwork.Time;

            if (requestedGrabbables.ContainsKey(view.ViewID)) {
                requestedGrabbables[view.ViewID] = lastRequestTime;
            }
            else {
                requestedGrabbables.Add(view.ViewID, lastRequestTime);
            }

            view.RequestOwnership();
        }


        /// <summary>
        /// Sets the sync values and moves the netwrok objects according to local player actions
        /// works on a lerp to allow for smooth syncing
        /// teleporting has been commented out as it creates a "laggy" movement
        /// </summary>
        /// <param name="moveTransform"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="syncStartRotation"></param>
        /// <param name="syncEndRotation"></param>
        /// <param name="syncValue"></param>
        void updateRemotePositionRotation(Transform moveTransform, Vector3 startPosition, Vector3 endPosition, Quaternion syncStartRotation, Quaternion syncEndRotation, float syncValue) {
            float dist = Vector3.Distance(startPosition, endPosition);

            // If far away just teleport there
            /*if (dist > 0.5f) {
                moveTransform.position = endPosition;
                moveTransform.rotation = syncEndRotation;
            }
            else {*/
                moveTransform.position = Vector3.Lerp(startPosition, endPosition, syncValue);
                moveTransform.rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncValue);
            //}
        }

       
        /// <summary>
        /// Serializes and shares all data pertaining to the user across the network
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="info"></param>
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            // This is our player, send our positions to the other players
            if (stream.IsWriting) {

                // Player Head / Hand Information
                stream.SendNext(PlayerHeadTransform.position);
                stream.SendNext(PlayerHeadTransform.rotation);
                stream.SendNext(PlayerLeftHandTransform.position);
                stream.SendNext(PlayerLeftHandTransform.rotation);
                stream.SendNext(PlayerRightHandTransform.position);
                stream.SendNext(PlayerRightHandTransform.rotation);
                stream.SendNext(PlayerJammerTransform.position);
                stream.SendNext(PlayerJammerTransform.rotation);
               

                // Hand Animator Info
               /* if(LeftHandController) {
                    stream.SendNext(LeftHandController.GripAmount);
                    stream.SendNext(LeftHandController.PointAmount);
                    stream.SendNext(LeftHandController.ThumbAmount);
                    stream.SendNext(LeftHandController.PoseId);
                    stream.SendNext(LeftHandController.grabber.HoldingItem);
                }
                if (RightHandController) {
                    stream.SendNext(RightHandController.GripAmount);
                    stream.SendNext(RightHandController.PointAmount);
                    stream.SendNext(RightHandController.ThumbAmount);
                    stream.SendNext(RightHandController.PoseId);
                    stream.SendNext(RightHandController.grabber.HoldingItem);
                }*/
            }
            else {
                // Remote player, receive data
                // Head
                this._syncHeadStartPosition = RemoteHeadTransform.position;
                this._syncHeadEndPosition = (Vector3)stream.ReceiveNext();
                this._syncHeadStartRotation = RemoteHeadTransform.rotation;
                this._syncHeadEndRotation = (Quaternion)stream.ReceiveNext();

                // Left Hand
                this._syncLHandStartPosition = RemoteLeftHandTransform.position;
                this._syncLHandEndPosition = (Vector3)stream.ReceiveNext();
                this._syncLHandStartRotation = RemoteLeftHandTransform.rotation;
                this._syncLHandEndRotation = (Quaternion)stream.ReceiveNext();

                // Right Hand
                this._syncRHandStartPosition = RemoteRightHandTransform.position;
                this._syncRHandEndPosition = (Vector3)stream.ReceiveNext();
                this._syncRHandStartRotation = RemoteRightHandTransform.rotation;
                this._syncRHandEndRotation = (Quaternion)stream.ReceiveNext();

                //Jammer
                this._syncJammerStartPos = RemoteJammerTransform.position;
                this._syncJammerEndPos = (Vector3)stream.ReceiveNext();
                this._syncJammerStartRot = RemoteJammerTransform.rotation;
                this._syncJammerEndRot = (Quaternion)stream.ReceiveNext();

                // Left Hand Animation Updates
                if(RemoteLeftHandAnimator) {
                    _syncLeftGripEnd = (float)stream.ReceiveNext();
                    _syncLeftPointEnd = (float)stream.ReceiveNext();
                    _syncLeftThumbEnd = (float)stream.ReceiveNext();

                    // Can set hand pose immediately
                    RemoteLeftHandAnimator.SetInteger("Pose", (int)stream.ReceiveNext());

                    _syncLeftHoldingItem = (bool)stream.ReceiveNext();
                }

                if (RemoteRightHandAnimator) {
                    _syncRightGripEnd = (float)stream.ReceiveNext();
                    _syncRightPointEnd = (float)stream.ReceiveNext();
                    _syncRightThumbEnd = (float)stream.ReceiveNext();

                    // Can set hand pose immediately
                    RemoteRightHandAnimator.SetInteger("Pose", (int)stream.ReceiveNext());

                    _syncRightHoldingItem = (bool)stream.ReceiveNext();
                }

                _syncTime = 0f;
                _syncDelay = Time.time - _lastSynchronizationTime;
                _lastSynchronizationTime = Time.time;
            }
        }

        // Handle Ownership Requests (Ex: Grabbable Ownership)
        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer) {

            bool amOwner = targetView.AmOwner || (targetView.Owner == null && PhotonNetwork.IsMasterClient);

           /* NetworkedGrabbable netGrabbable = targetView.gameObject.GetComponent<NetworkedGrabbable>();
            if (netGrabbable != null) {
                // Authorize transfer of ownership if we're not holding it
                if (amOwner && !netGrabbable.BeingHeld) {
                    targetView.TransferOwnership(requestingPlayer.ActorNumber);
                    return;
                }
            }*/
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player requestingPlayer) {
            // Debug.Log("OnOwnershipTransfered to Player " + requestingPlayer);
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player requestingPlayer) {
            // Debug.Log("OnOwnershipTransferFailed for Player " + requestingPlayer);
        }

          /// <summary>
          /// PUN Remote procedure calls to change all gloves and vics across the network
          /// </summary>
          /// <param name="slectedGlove"></param>
        [PunRPC]
        public void ChangeAvatarGlovesPerSelections(int slectedGlove)
        {
            switch (slectedGlove)
            {
                case 1:
                    greenGlovesL.SetActive(true);
                    greenGlovesR.SetActive(true);
                    darkHandsL.SetActive(false);
                    darkHandsR.SetActive(false);
                    lightHalfGlovesL.SetActive(false);
                    lightHalfGlovesR.SetActive(false);
                    tanHalfGlovesL.SetActive(false);
                    tanHalfGlovesR.SetActive(false);
                    caucHandsL.SetActive(false);
                    caucHandsR.SetActive(false);
                    break;

                case 2:
                    greenGlovesL.SetActive(false);
                    greenGlovesR.SetActive(false);
                    darkHandsL.SetActive(false);
                    darkHandsR.SetActive(false);
                    lightHalfGlovesL.SetActive(true);
                    lightHalfGlovesR.SetActive(true);
                    tanHalfGlovesL.SetActive(false);
                    tanHalfGlovesR.SetActive(false);
                    caucHandsL.SetActive(false);
                    caucHandsR.SetActive(false);
                    break;

                case 3:
                    greenGlovesL.SetActive(false);
                    greenGlovesR.SetActive(false);
                    darkHandsL.SetActive(false);
                    darkHandsR.SetActive(false);
                    lightHalfGlovesL.SetActive(false);
                    lightHalfGlovesR.SetActive(false);
                    tanHalfGlovesL.SetActive(true);
                    tanHalfGlovesR.SetActive(true);
                    caucHandsL.SetActive(false);
                    caucHandsR.SetActive(false);
                    break;

                case 4:
                    greenGlovesL.SetActive(false);
                    greenGlovesR.SetActive(false);
                    darkHandsL.SetActive(false);
                    darkHandsR.SetActive(false);
                    lightHalfGlovesL.SetActive(false);
                    lightHalfGlovesR.SetActive(false);
                    tanHalfGlovesL.SetActive(false);
                    tanHalfGlovesR.SetActive(false);
                    caucHandsL.SetActive(true);
                    caucHandsR.SetActive(true);
                    break;

                case 5:
                    greenGlovesR.SetActive(false);
                    greenGlovesL.SetActive(false);
                    darkHandsL.SetActive(true);
                    darkHandsR.SetActive(true);
                    lightHalfGlovesL.SetActive(false);
                    lightHalfGlovesR.SetActive(false);
                    tanHalfGlovesL.SetActive(false);
                    tanHalfGlovesR.SetActive(false);
                    caucHandsL.SetActive(false);
                    caucHandsR.SetActive(false);
                    break;
            }
        }
        [PunRPC]
        public void ChangeAvatarVicPerSelections(int slectedVic)
        {
            switch (slectedVic)
            {
                case 0:

                    jammerForks.SetActive(false);
                    jammerr2D2.SetActive(false);
                    break;

                case 1:
                    jammerForks.SetActive(true);
                    jammerr2D2.SetActive(false);
                    break;

                case 2:
                    jammerForks.SetActive(false);
                    jammerr2D2.SetActive(true);
                    break;

               
            }
        }
#endif
    }
   
}