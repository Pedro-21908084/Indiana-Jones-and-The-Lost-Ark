﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniGLTF
{
    [Flags]
    public enum TextureLoadFlags
    {
        None = 0,
        Used = 1,
        External = 1 << 1,
    }

    public struct TextureLoadInfo
    {
        public readonly Texture2D Texture;
        public readonly TextureLoadFlags Flags;
        public bool IsUsed => Flags.HasFlag(TextureLoadFlags.Used);
        public bool IsExternal => Flags.HasFlag(TextureLoadFlags.External);

        public bool IsSubAsset => IsUsed && !IsExternal;

        public TextureLoadInfo(Texture2D texture, bool used, bool isExternal)
        {
            Texture = texture;
            var flags = TextureLoadFlags.None;
            if (used)
            {
                flags |= TextureLoadFlags.Used;
            }
            if (isExternal)
            {
                flags |= TextureLoadFlags.External;
            }
            Flags = flags;
        }
    }

    public delegate Task<TextureLoadInfo> LoadTextureAsyncFunc(IAwaitCaller awaitCaller, int index, bool used);
    public delegate Task<Texture2D> GetTextureAsyncFunc(IAwaitCaller awaitCaller, glTF gltf, GetTextureParam param);
    public class TextureFactory : IDisposable
    {
        public readonly Dictionary<string, Texture2D> ExternalMap;

        public bool TryGetExternal(GetTextureParam param, bool used, out Texture2D external)
        {
            if (param.Index0.HasValue && ExternalMap != null)
            {
                var cacheName = param.ConvertedName;
                if (param.TextureType == GetTextureParam.TextureTypes.NormalMap)
                {
                    cacheName = param.GltflName;
                    if (m_textureCache.TryGetValue(cacheName, out TextureLoadInfo normalInfo))
                    {
                        external = normalInfo.Texture;
                        return true;
                    }
                }
                if (ExternalMap.TryGetValue(cacheName, out external))
                {
                    m_textureCache.Add(cacheName, new TextureLoadInfo(external, used, true));
                    return true;
                }
            }
            external = default;
            return false;
        }

        public UnityPath ImageBaseDir { get; set; }

        public TextureFactory(LoadTextureAsyncFunc loadTextureAsync,
            IEnumerable<(string, UnityEngine.Object)> externalMap)
        {
            LoadTextureAsync = loadTextureAsync;
            if (externalMap != null)
            {
                ExternalMap = externalMap
                    .Select(kv => (kv.Item1, kv.Item2 as Texture2D))
                    .Where(kv => kv.Item2 != null)
                    .ToDictionary(kv => kv.Item1, kv => kv.Item2);
            }
        }

        public void Dispose()
        {
            Action<UnityEngine.Object> destroy = UnityResourceDestroyer.DestroyResource();
            foreach (var kv in m_textureCache)
            {
                if (!kv.Value.IsExternal)
                {
#if VRM_DEVELOP
                    // Debug.Log($"Destroy {kv.Value.Texture}");
#endif
                    destroy(kv.Value.Texture);
                }
            }
            m_textureCache.Clear();
        }

        /// <summary>
        /// 所有権(Dispose権)を移譲する
        /// </summary>
        /// <param name="take"></param>
        public void TransferOwnership(TakeOwnershipFunc take)
        {
            var keys = new List<string>();
            foreach (var x in m_textureCache)
            {
                if (x.Value.IsUsed && !x.Value.IsExternal)
                {
                    // マテリアルから参照されていて
                    // 外部のAssetからロードしていない。
                    if (take(x.Value.Texture))
                    {
                        keys.Add(x.Key);
                    }
                }
            }
            foreach (var x in keys)
            {
                m_textureCache.Remove(x);
            }
        }

        Dictionary<string, TextureLoadInfo> m_textureCache = new Dictionary<string, TextureLoadInfo>();

        public IEnumerable<TextureLoadInfo> Textures => m_textureCache.Values;

        public LoadTextureAsyncFunc LoadTextureAsync;

        async Task<TextureLoadInfo> GetOrCreateBaseTexture(IAwaitCaller awaitCaller, glTF gltf, int textureIndex, bool used)
        {
            var name = gltf.textures[textureIndex].name;
            if (!m_textureCache.TryGetValue(name, out TextureLoadInfo cacheInfo))
            {
                cacheInfo = await LoadTextureAsync(awaitCaller, textureIndex, used);
                m_textureCache.Add(name, cacheInfo);
            }
            return cacheInfo;
        }

        /// <summary>
        /// テクスチャーをロード、必要であれば変換して返す。
        /// 同じものはキャッシュを返す
        /// </summary>
        /// <param name="texture_type">変換の有無を判断する: METALLIC_GLOSS_PROP</param>
        /// <param name="roughnessFactor">METALLIC_GLOSS_PROPの追加パラメーター</param>
        /// <param name="indices">gltf の texture index</param>
        /// <returns></returns>
        public async Task<Texture2D> GetTextureAsync(IAwaitCaller awaitCaller, glTF gltf, GetTextureParam param)
        {
            if (m_textureCache.TryGetValue(param.ConvertedName, out TextureLoadInfo cacheInfo))
            {
                return cacheInfo.Texture;
            }
            if (TryGetExternal(param, true, out Texture2D external))
            {
                return external;
            }

            switch (param.TextureType)
            {
                case GetTextureParam.TextureTypes.NormalMap:
                    {
                        var baseTexture = await GetOrCreateBaseTexture(awaitCaller, gltf, param.Index0.Value, false);
                        var converted = NormalConverter.Import(baseTexture.Texture);
                        converted.name = param.ConvertedName;
                        var info = new TextureLoadInfo(converted, true, false);
                        m_textureCache.Add(converted.name, info);
                        return info.Texture;
                    }

                case GetTextureParam.TextureTypes.StandardMap:
                    {
                        TextureLoadInfo baseTexture = default;
                        if (param.Index0.HasValue)
                        {
                            baseTexture = await GetOrCreateBaseTexture(awaitCaller, gltf, param.Index0.Value, false);
                        }
                        TextureLoadInfo occlusionBaseTexture = default;
                        if (param.Index1.HasValue)
                        {
                            occlusionBaseTexture = await GetOrCreateBaseTexture(awaitCaller, gltf, param.Index1.Value, false);
                        }
                        var converted = OcclusionMetallicRoughnessConverter.Import(baseTexture.Texture, param.MetallicFactor, param.RoughnessFactor, occlusionBaseTexture.Texture);
                        converted.name = param.ConvertedName;
                        var info = new TextureLoadInfo(converted, true, false);
                        m_textureCache.Add(converted.name, info);
                        return info.Texture;
                    }

                default:
                    {
                        var baseTexture = await GetOrCreateBaseTexture(awaitCaller, gltf, param.Index0.Value, true);
                        return baseTexture.Texture;
                    }

                    throw new NotImplementedException();
            }
        }
    }
}
