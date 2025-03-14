using NUnit.Framework;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class TextboxTests
{
    private GameObject textboxObject;
    private Textbox textbox;
    private GameObject dialogBox;
    private Text dialogText;
    
    [SetUp]
    public void Setup()
    {
        textboxObject = new GameObject("TextboxTestObject");
        textbox = textboxObject.AddComponent<Textbox>();
        dialogBox = new GameObject("DialogBox");
        GameObject dialogTextObj = new GameObject("DialogText");
        dialogText = dialogTextObj.AddComponent<Text>();
        dialogText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        dialogTextObj.transform.SetParent(dialogBox.transform);
        textbox.dialogBox = dialogBox;
        textbox.dialogText = dialogText;
        textbox.dialogs = new string[] { "Hello", "World" };
        dialogBox.SetActive(false);
    }
    
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(textboxObject);
        Object.DestroyImmediate(dialogBox);
    }
    
    [Test]
    public void OnTriggerEnter2DTest()
    {
        GameObject playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        Collider2D playerCollider = playerObject.AddComponent<BoxCollider2D>();
        LogAssert.Expect(LogType.Assert, "Assertion failed on expression: 'ShouldRunBehaviour()'");
        textbox.SendMessage("OnTriggerEnter2D", playerCollider, SendMessageOptions.DontRequireReceiver);
        
        Assert.IsTrue(textbox.playerInRange, "OnTriggerEnter2D should set playerInRange to true.");
        
        Object.DestroyImmediate(playerObject);
    }
    
    [Test]
    public void OnTriggerExit2D_ResetsDialogAndPlayerInRange()
    {
        
        GameObject playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        Collider2D playerCollider = playerObject.AddComponent<BoxCollider2D>();
        dialogBox.SetActive(true);
        dialogText.text = "Some Text";
        FieldInfo currentDialogIndexField = typeof(Textbox).GetField("currentDialogIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        currentDialogIndexField.SetValue(textbox, 1);
        textbox.playerInRange = true;
        LogAssert.Expect(LogType.Assert, "Assertion failed on expression: 'ShouldRunBehaviour()'");
        textbox.SendMessage("OnTriggerExit2D", playerCollider, SendMessageOptions.DontRequireReceiver);
        Assert.IsFalse(textbox.playerInRange, "OnTriggerExit2D should set playerInRange to false.");
        Assert.IsFalse(dialogBox.activeSelf, "OnTriggerExit2D should deactivate the dialog box.");
        int currentDialogIndex = (int)currentDialogIndexField.GetValue(textbox);
        Assert.AreEqual(0, currentDialogIndex, "OnTriggerExit2D should reset currentDialogIndex to 0.");
        
        Object.DestroyImmediate(playerObject);
    }
}
