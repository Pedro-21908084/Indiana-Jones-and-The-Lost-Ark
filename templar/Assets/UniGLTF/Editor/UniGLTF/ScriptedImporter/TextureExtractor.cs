using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace UniGLTF
{
    public class TextureExtractor
    {
        const string TextureDirName = "Textures";

        GltfParser m_parser;
        public GltfParser Parser => m_parser;

        public glTF GLTF => m_parser.GLTF;
        public IStorage Storage => m_parser.Storage;

        public readonly Dictionary<UnityPath, GetTextureParam> Textures = new Dictionary<UnityPath, GetTextureParam>();
        UnityEngine.Texture2D[] m_subAssets;
        UnityPath m_textureDirectory;

        public TextureExtractor(GltfParser parser, UnityPath textureDirectory, UnityEngine.Texture2D[] subAssets)
        {
            m_parser = parser;
            m_textureDirectory = textureDirectory;
            m_textureDirectory.EnsureFolder();
            m_subAssets = subAssets;
        }

        public static string GetExt(string mime, string uri)
        {
            switch (mime)
            {
                case "image/png": return ".png";
                case "image/jpeg": return ".jpg";
            }

            return Path.GetExtension(uri).ToLower();
        }

        public void Extract(GetTextureParam param, bool hasUri)
        {
            if (Textures.Values.Contains(param))
            {
                return;
            }

            var subAsset = m_subAssets.FirstOrDefault(x => x.name == param.ConvertedName);
            UnityPath targetPath = default;

            if (hasUri && !param.ExtractConverted)
            {
                var gltfTexture = GLTF.textures[param.Index0.Value];
                var gltfImage = GLTF.images[gltfTexture.source];
                var ext = GetExt(gltfImage.mimeType, gltfImage.uri);
                targetPath = m_textureDirectory.Child($"{param.GltflName}{ext}");
            }
            else
            {

                switch (param.TextureType)
                {
                    case GetTextureParam.TextureTypes.StandardMap:
                        {
                            // write converted texture
                            targetPath = m_textureDirectory.Child($"{param.ConvertedName}.png");
                            File.WriteAllBytes(targetPath.FullPath, subAsset.EncodeToPNG().ToArray());
                            targetPath.ImportAsset();
                            break;
                        }

                    default:
                        {
                            // write original bytes
                            var gltfTexture = GLTF.textures[param.Index0.Value];
                            var gltfImage = GLTF.images[gltfTexture.source];
                            var ext = GetExt(gltfImage.mimeType, gltfImage.uri);
                            targetPath = m_textureDirectory.Child($"{param.GltflName}{ext}");
                            File.WriteAllBytes(targetPath.FullPath, GLTF.GetImageBytes(Storage, gltfTexture.source).ToArray());
                            targetPath.ImportAsset();
                            break;
                        }
                }
            }

            Textures.Add(targetPath, param);
        }

        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 
        /// * Texture(.png etc...)をディスクに書き出す
        /// * EditorApplication.delayCall で処理を進めて 書き出した画像が Asset として成立するのを待つ
        /// * 書き出した Asset から TextureImporter を取得して設定する
        /// 
        /// </summary>
        /// <param name="importer"></param>
        /// <param name="dirName"></param>
        /// <param name="onCompleted"></param>
        public static void ExtractTextures(GltfParser parser, UnityPath textureDirectory,
            TextureEnumerator textureEnumerator, Texture2D[] subAssets, Action<Texture2D> addRemap,
            Action<IEnumerable<UnityPath>> onCompleted = null)
        {
            var extractor = new TextureExtractor(parser, textureDirectory, subAssets);
            foreach (var x in textureEnumerator(extractor.GLTF))
            {
                var gltfTexture = extractor.GLTF.textures[x.Index0.Value];
                var gltfImage = extractor.GLTF.images[gltfTexture.source];
                extractor.Extract(x, !string.IsNullOrEmpty(gltfImage.uri));
            }

            EditorApplication.delayCall += () =>
            {
                // Wait for the texture assets to be imported

                foreach (var kv in extractor.Textures)
                {
                    var targetPath = kv.Key;
                    var param = kv.Value;

                    // remap
                    var externalObject = targetPath.LoadAsset<Texture2D>();
                    if (externalObject != null)
                    {
                        addRemap(externalObject);
                    }
                }

                if (onCompleted != null)
                {
                    onCompleted(extractor.Textures.Keys);
                }
            };
        }
    }
}
