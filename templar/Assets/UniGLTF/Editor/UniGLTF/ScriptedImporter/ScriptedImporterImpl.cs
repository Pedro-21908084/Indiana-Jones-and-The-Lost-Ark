using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif


namespace UniGLTF
{
    public static class ScriptedImporterImpl
    {
        /// <summary>
        /// glb をパースして、UnityObject化、さらにAsset化する
        /// </summary>
        /// <param name="scriptedImporter"></param>
        /// <param name="context"></param>
        /// <param name="reverseAxis"></param>
        public static void Import(ScriptedImporter scriptedImporter, AssetImportContext context, Axises reverseAxis)
        {
#if VRM_DEVELOP            
            Debug.Log("OnImportAsset to " + scriptedImporter.assetPath);
#endif

            //
            // Parse(parse glb, parser gltf json)
            //
            var parser = new GltfParser();
            parser.ParsePath(scriptedImporter.assetPath);

            //
            // Import(create unity objects)
            //
            var externalObjectMap = scriptedImporter.GetExternalObjectMap().Select(kv => (kv.Value.name, kv.Value)).ToArray();
            
            var externalTextures = EnumerateTexturesFromUri(externalObjectMap, parser, UnityPath.FromUnityPath(scriptedImporter.assetPath).Parent).ToArray();

            using (var loaded = new ImporterContext(parser, null, externalObjectMap.Concat(externalTextures)))
            {
                // settings TextureImporters
                foreach (var textureInfo in GltfTextureEnumerator.Enumerate(parser.GLTF))
                {
                    TextureImporterConfigurator.Configure(textureInfo, loaded.TextureFactory.ExternalMap);
                }

                loaded.InvertAxis = reverseAxis;
                loaded.Load();
                loaded.ShowMeshes();

                loaded.TransferOwnership(o =>
                {
#if VRM_DEVELOP
                    Debug.Log($"[{o.GetType().Name}] {o.name} will not destroy");
#endif

                    context.AddObjectToAsset(o.name, o);
                    if (o is GameObject)
                    {
                        // Root GameObject is main object
                        context.SetMainObject(loaded.Root);
                    }

                    return true;
                });
            }
        }

        public static IEnumerable<(string, UnityEngine.Object)> EnumerateTexturesFromUri(IEnumerable<(string, UnityEngine.Object)> exclude,
            GltfParser parser, UnityPath dir)
        {
            var used = new HashSet<Texture2D>();
            foreach (var texParam in GltfTextureEnumerator.Enumerate(parser.GLTF))
            {
                switch (texParam.TextureType)
                {
                    case GetTextureParam.TextureTypes.StandardMap:
                        break;

                    default:
                        {
                            var gltfTexture = parser.GLTF.textures.First(y => y.name == texParam.GltflName);
                            var gltfImage = parser.GLTF.images[gltfTexture.source];
                            if (!string.IsNullOrEmpty(gltfImage.uri) && !gltfImage.uri.StartsWith("data:"))
                            {
                                var child = dir.Child(gltfImage.uri);
                                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(child.Value);
                                if (asset == null)
                                {
                                    throw new System.IO.FileNotFoundException($"{child}");
                                }

                                if (exclude != null && exclude.Any(kv => kv.Item2.name == asset.name))
                                {
                                    // exclude. skip
                                    var a = 0;
                                }
                                else{
                                    if(used.Add(asset)){
                                        yield return (asset.name, asset);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
