using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class MeleeEnemyDamageTests
{
    private GameObject enemyGameObject;
    private GameObject playerGameObject;
    private MeleeEnemyDamage meleeEnemyDamage;
    private HealthManager healthManager;
    private ParticleSystem mockParticleSystem;

    [SetUp]
    public void SetUp()
    {
        // Create enemy GameObject with MeleeEnemyDamage component
        enemyGameObject = new GameObject("Enemy");
        meleeEnemyDamage = enemyGameObject.AddComponent<MeleeEnemyDamage>();

        // Add a trigger collider to the enemy
        var enemyCollider = enemyGameObject.AddComponent<BoxCollider2D>();
        enemyCollider.isTrigger = true;

        // Create player GameObject with HealthManager
        playerGameObject = new GameObject("Player");
        playerGameObject.tag = "Player";
        healthManager = playerGameObject.AddComponent<HealthManager>();

        // Create mock UI elements for HealthManager (required dependencies)
        var healthBarObject = new GameObject("HealthBar");
        var healthBarImage = healthBarObject.AddComponent<Image>();
        healthManager.healthBar = healthBarImage;

        // Add collider to player
        var playerCollider = playerGameObject.AddComponent<BoxCollider2D>();

        // Create mock particle system
        var particleGameObject = new GameObject("ParticleSystem");
        mockParticleSystem = particleGameObject.AddComponent<ParticleSystem>();

        // Configure MeleeEnemyDamage
        meleeEnemyDamage.damageAmount = 10f;
        meleeEnemyDamage.damageDelay = 1f;
        meleeEnemyDamage.attackParticleSystem = mockParticleSystem;
        meleeEnemyDamage.particleOffset = Vector3.zero;

        // Position objects (only if needed for specific tests)
        enemyGameObject.transform.position = Vector3.zero;
        playerGameObject.transform.position = new Vector3(1f, 0f, 0f);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up all created objects
        if (enemyGameObject != null)
            Object.DestroyImmediate(enemyGameObject);
        if (playerGameObject != null)
            Object.DestroyImmediate(playerGameObject);
        if (mockParticleSystem != null && mockParticleSystem.gameObject != null)
            Object.DestroyImmediate(mockParticleSystem.gameObject);
        if (healthManager.healthBar != null && healthManager.healthBar.gameObject != null)
            Object.DestroyImmediate(healthManager.healthBar.gameObject);
    }

    [Test]
    public void DamageAmount_ShouldBeConfigurable()
    {
        // Arrange
        float expectedDamage = 15f;

        // Act
        meleeEnemyDamage.damageAmount = expectedDamage;

        // Assert
        Assert.AreEqual(expectedDamage, meleeEnemyDamage.damageAmount);
    }
}