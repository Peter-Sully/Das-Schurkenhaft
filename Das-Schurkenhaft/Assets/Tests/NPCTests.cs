#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;

public class NPCTests
{
    private GameObject npcObject;
    private NPC npc;
    private GameObject boundsObject;
    private BoxCollider2D boundsCollider;

    [SetUp]
    public void Setup()
    {
       
        npcObject = new GameObject("NPC_TestObject");
        npc = npcObject.AddComponent<NPC>();

        Animator animator = npcObject.AddComponent<Animator>();
        Rigidbody2D rb = npcObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic; 
        rb.interpolation = RigidbodyInterpolation2D.None;

#if UNITY_EDITOR
        AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPathWithClip("Assets/Dummy.controller", new AnimationClip());
        ac.AddParameter("MoveX", AnimatorControllerParameterType.Float);
        ac.AddParameter("MoveY", AnimatorControllerParameterType.Float);
        animator.runtimeAnimatorController = ac;
#endif

        boundsObject = new GameObject("Bounds");
        boundsCollider = boundsObject.AddComponent<BoxCollider2D>();
        boundsCollider.size = new Vector2(10f, 10f);
        npc.bounds = boundsCollider;

        npc.speed = 1f;
        npc.minMoveTime = 0.5f;
        npc.maxMoveTime = 0.5f;
        npc.minWaitTime = 0.5f;
        npc.maxWaitTime = 0.5f;

        MethodInfo startMethod = typeof(NPC).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic);
        startMethod.Invoke(npc, null);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(npcObject);
        Object.DestroyImmediate(boundsObject);
    }

    [Test]
    public void ChangeDirectionTest()
    {
        MethodInfo changeDirMethod = typeof(NPC).GetMethod("ChangeDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        changeDirMethod.Invoke(npc, null);

        FieldInfo dirField = typeof(NPC).GetField("directionVector", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector3 currentDirection = (Vector3)dirField.GetValue(npc);

        Animator animator = npcObject.GetComponent<Animator>();
        float moveX = animator.GetFloat("MoveX");
        float moveY = animator.GetFloat("MoveY");
        Assert.AreEqual(currentDirection.x, moveX, 0.001f, "Animator MoveX should match the current direction's x component.");
        Assert.AreEqual(currentDirection.y, moveY, 0.001f, "Animator MoveY should match the current direction's y component.");
    }

    [Test]
    public void ChooseDifferentDirectionTest()
    {
        FieldInfo dirField = typeof(NPC).GetField("directionVector", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector3 initialDirection = (Vector3)dirField.GetValue(npc);

        MethodInfo chooseDiffMethod = typeof(NPC).GetMethod("ChooseDifferentDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        chooseDiffMethod.Invoke(npc, null);

        Vector3 newDirection = (Vector3)dirField.GetValue(npc);
        Assert.AreNotEqual(initialDirection, newDirection, "NPC should choose a different direction when ChooseDifferentDirection is called.");
    }

    [Test]
    public void OnCollisionTest()
    {
        FieldInfo dirField = typeof(NPC).GetField("directionVector", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector3 oldDirection = (Vector3)dirField.GetValue(npc);

        MethodInfo collisionMethod = typeof(NPC).GetMethod("OnCollisionEnter2D", BindingFlags.NonPublic | BindingFlags.Instance);
        collisionMethod.Invoke(npc, new object[] { null });

        Vector3 newDirection = (Vector3)dirField.GetValue(npc);
        Assert.AreNotEqual(oldDirection, newDirection, "NPC should choose a different direction when a collision occurs.");
    }
}
