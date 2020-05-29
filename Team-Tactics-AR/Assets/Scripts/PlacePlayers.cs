using UnityEngine;

public class PlacePlayers : MonoBehaviour
{
    [SerializeField] private Camera arCamera = null;
    [SerializeField] private GameObject[] players = null;
    [SerializeField] private GameObject yellowTeamActive = null;
    [SerializeField] private Material ghostMaterial = null;
    [SerializeField] private GameObject arrowUI = null;
    [SerializeField] private GameObject crossUI = null;
    [SerializeField] private GameObject rotateUI = null;


    //private Vector2 touchPosition;
    private int currentIndexAnimation = 0;

    private GameObject ghostPlayer;
    private bool isAGhostPlaced = false;
    private Transform child;

    private void Update()
    {

        PlacePlayerHandler();
    }

    private void PlacePlayerHandler()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.CompareTag("Indicator"))
                    {
                        // Activating an effect in the place indicator for the user to know that it is selected.
                        if(ghostPlayer != null)
                        {
                            Deselect();
                        }

                        child = hitObject.transform.Find("Holo");
                        child.gameObject.SetActive(true);
                        PlaceGhost();
                    }
                    
                }
            }
        }
    }

    private void PlaceGhost()
    {
        isAGhostPlaced = true;
        ghostPlayer = Instantiate(players[currentIndexAnimation], child.position, child.rotation);
        ghostPlayer.transform.Find("H_DDS_MidRes").gameObject.GetComponent<Renderer>().material = ghostMaterial;
        ToggleUIActiveIcons(true);
    }

    public void SwitchAnimation(int indexOfAnimation)
    {
        if (indexOfAnimation <= 3)
        {
            if (!yellowTeamActive.activeSelf)
            {
                currentIndexAnimation = indexOfAnimation + 4;
            }
            else
            {
                currentIndexAnimation = indexOfAnimation;
            }
        }
        else
        {
            currentIndexAnimation = indexOfAnimation;
        }

        // If there is already a player selected but not placed:
        if (ghostPlayer != null)
        {
            Destroy(ghostPlayer);
            ghostPlayer = Instantiate(players[currentIndexAnimation], child.position, child.rotation);
            ghostPlayer.transform.Find("H_DDS_MidRes").gameObject.GetComponent<Renderer>().material = ghostMaterial;
        }
    }

    public void PlacePlayer()
    {
        if (isAGhostPlaced)
        {
            Instantiate(players[currentIndexAnimation], ghostPlayer.transform.position, ghostPlayer.transform.rotation);
            Deselect();
        }
    }

    public void Deselect()
    {
        Destroy(ghostPlayer);
        isAGhostPlaced = false;
        child.gameObject.SetActive(false);
        ToggleUIActiveIcons(false);
    }

    private void ToggleUIActiveIcons(bool setIcons)
    {
        arrowUI.SetActive(setIcons);
        crossUI.SetActive(setIcons);
        rotateUI.SetActive(setIcons);
    }

    public void RotatePlayer()
    {
        Quaternion newRotation = new Quaternion(ghostPlayer.transform.localRotation.x, ghostPlayer.transform.localRotation.y + 180, ghostPlayer.transform.localRotation.z, 0);
        ghostPlayer.transform.localRotation = newRotation;
    }
}