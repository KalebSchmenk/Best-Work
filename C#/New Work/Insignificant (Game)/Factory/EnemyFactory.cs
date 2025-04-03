using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// DESIGN PATTERN: Registration Factory
/// </summary>
public class EnemyFactory : MonoBehaviour
{
    private Dictionary<string, ProductPool> products = new Dictionary<string, ProductPool>();

    /// <summary>
    /// Register a new product to this factory.
    /// </summary>
    /// <param name="productName">Name of the product for accessing later.</param>
    /// <param name="productObj">Product gameobject reference.</param>
    public void RegisterProduct(string productName, GameObject productObj)
    {
        // Create an object pool for this product
        GenericGameObjectPool genericGameObjectPool = this.AddComponent<GenericGameObjectPool>();

        // Create a new enclosed object with the product obj reference and its pool
        ProductPool productPool = new ProductPool(productObj, genericGameObjectPool);

        // Add the new enclosed object and its name to the dictionary of registered products
        products.Add(productName, productPool);
    }

    /// <summary>
    /// Gets a new enemy from the product pool and sets it's factory tag.
    /// If passed argument does not exist in dictionary of products, defaults to easy enemy.
    /// </summary>
    /// <param name="productName">Product to create a new instance of.</param>
    /// <returns>Enemy instance.</returns>
    public GameObject CreateEnemy(string productName)
    {
        GameObject enemy;

        // Takes an enemy instance from its pool if the passed product argument exists in dictionary.
        // Returns an easy enemy as default.
        if (products.ContainsKey(productName))
        {
            enemy = products[productName].productPool.Take();
        }
        else
        {
            Debug.LogWarning($"The product name {productName} has not been registered. Defaulting to creating an easy enemy");
            enemy = products["EasyEnemy"].productPool.Take();
        }

        // Adding a factory tag for later referencing if it doesn't exist
        // Factory tag = prototype name
        if (enemy.TryGetComponent<FactoryTag>(out FactoryTag tag))
        {
            tag.ChangeTag(productName);
        }
        else
        {
            FactoryTag fTag = enemy.AddComponent<FactoryTag>();
            fTag.ChangeTag(productName);           
        }

        return enemy;
    }

    /// <summary>
    /// Returns an enemy to the factory to be pooled. Enemy must be factory tagged.
    /// </summary>
    /// <param name="returningEnemy">Enemy to return to factory.</param>
    public void ReturnEnemy(GameObject returningEnemy)
    {
        if (returningEnemy.TryGetComponent<FactoryTag>(out FactoryTag fTag))
        {
            if (products.ContainsKey(fTag.FTag))
            {
                products[fTag.FTag].productPool.Return(returningEnemy);
            }
            else
            {
                Debug.LogWarning($"Enemy Factory Tag {fTag} is not in the dictionary of products. Ignoring return request...");
            }
        }
        else
        {
            Debug.LogWarning("Returning gameobject is not Factory Tagged. Ignoring return request...");
        }
    }
}

/// <summary>
/// Pool of product gameobjects
/// </summary>
public class ProductPool
{
    public ProductPool(GameObject product, GenericGameObjectPool productPool) 
    { 
        this.product = product;
        this.productPool = productPool;

        productPool.Init(product);
    }

    public GameObject product;
    public GenericGameObjectPool productPool;
}
