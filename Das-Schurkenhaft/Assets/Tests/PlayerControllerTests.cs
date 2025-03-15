using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTests
{
    private GameObject playerGameObject;
    private PlayerController playerController;
    private SPUM_Prefabs spumPrefabs;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the PlayerController component
        playerGameObject = new GameObject();
        playerController = playerGameObject.AddComponent<PlayerController>();

        // Create a mock SPUM_Prefabs object and set it up
        spumPrefabs = new GameObject().AddComponent<SPUM_Prefabs>();
        playerController._prefabs = spumPrefabs;

        // Initialize Rigidbody2D
        playerController.gameObject.AddComponent<Rigidbody2D>();

        // Initialize InputAction
        playerController.MoveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        playerController.MoveAction.Enable();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerGameObject);
        Object.DestroyImmediate(spumPrefabs.gameObject);
    }

    [Test]
    public void PlayerController_Initialization_SetsDefaultValues()
    {
        // Assert
        Assert.AreEqual(10, playerController.GetHealth());
        Assert.AreEqual(3.0f, playerController.speed);
        Assert.AreEqual(1, playerController.strength);
        Assert.AreEqual(1, playerController.defense);
    }

    [Test]
    public void PlayerController_ChangeHealth_UpdatesHealthWithinBounds()
    {
        playerController.ChangeHealth(-3);

        Assert.AreEqual(7, playerController.GetHealth());

        playerController.ChangeHealth(10);

        Assert.AreEqual(10, playerController.GetHealth());

        playerController.ChangeHealth(-15);

        Assert.AreEqual(0, playerController.GetHealth());
    }

    [Test]
    public void PlayerController_ChangeSpeed_UpdatesSpeed()
    {
        playerController.ChangeSpeed(2.0f);

        Assert.AreEqual(5.0f, playerController.speed);
    }

    [Test]
    public void PlayerController_ChangeStrength_UpdatesStrength()
    {
        playerController.ChangeStrength(2);

        Assert.AreEqual(3, playerController.strength);
    }

    [Test]
    public void PlayerController_ChangeDefense_UpdatesDefense()
    {
        playerController.ChangeDefense(2);

        Assert.AreEqual(3, playerController.defense);
    }


}
