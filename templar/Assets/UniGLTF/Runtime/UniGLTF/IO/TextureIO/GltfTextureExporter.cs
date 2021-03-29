﻿using UnityEngine;

namespace UniGLTF
{
    public static class GltfTextureExporter
    {

        /// <summary>
        /// 画像のバイト列を得る
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        static (byte[] bytes, string mine) GetBytesWithMime(Texture2D texture)
        {
#if UNITY_EDITOR
            var path = UnityPath.FromAsset(texture);
            if (path.IsUnderAssetsFolder)
            {
                if (path.Extension == ".png")
                {
                    return
                    (
                        System.IO.File.ReadAllBytes(path.FullPath),
                        "image/png"
                    );
                }
                if (path.Extension == ".jpg")
                {
                    return
                    (
                        System.IO.File.ReadAllBytes(path.FullPath),
                        "image/jpeg"
                    );
                }
            }
#endif

            return
            (
                texture.EncodeToPNG(),
                "image/png"
            );
        }

        /// <summary>
        /// gltf に texture を足す
        /// 
        /// * textures
        /// * samplers
        /// * images
        /// * bufferViews
        /// 
        /// を更新し、textures の index を返す
        /// 
        /// </summary>
        /// <param name="gltf"></param>
        /// <param name="bufferIndex"></param>
        /// <param name="texture"></param>
        /// <returns>gltf texture index</returns>
        public static int PushGltfTexture(this glTF gltf, int bufferIndex, Texture2D texture)
        {
            var bytesWithMime = GetBytesWithMime(texture);

            // add view
            var view = gltf.buffers[bufferIndex].Append(bytesWithMime.bytes, glBufferTarget.NONE);
            var viewIndex = gltf.AddBufferView(view);

            // add image
            var imageIndex = gltf.images.Count;
            gltf.images.Add(new glTFImage
            {
                name = GetTextureParam.RemoveSuffix(texture.name),
                bufferView = viewIndex,
                mimeType = bytesWithMime.mine,
            });

            // add sampler
            var samplerIndex = gltf.samplers.Count;
            var sampler = TextureSamplerUtil.Export(texture);
            gltf.samplers.Add(sampler);

            // add texture
            var textureIndex = gltf.textures.Count;
            gltf.textures.Add(new glTFTexture
            {
                sampler = samplerIndex,
                source = imageIndex,
            });

            return textureIndex;
        }
    }
}
