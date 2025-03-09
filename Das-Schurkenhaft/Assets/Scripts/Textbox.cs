using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
   
    public string[] dialogs;
    
    private int currentDialogIndex = 0;
    public bool playerInRange;

    void Start() { }

    
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                currentDialogIndex++;
                if (currentDialogIndex < dialogs.Length)
                {
                    dialogText.text = dialogs[currentDialogIndex];
                }
                else
                {
                    dialogBox.SetActive(false);
                    currentDialogIndex = 0;
                }
            }
            else
            {
                dialogBox.SetActive(true);
                currentDialogIndex = 0;
                if(dialogs.Length > 0)
                {
                    dialogText.text = dialogs[currentDialogIndex];
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
            currentDialogIndex = 0;
        }
    }
}
