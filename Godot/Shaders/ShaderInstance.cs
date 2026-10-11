using Godot;
using System.Data;

namespace ProjectTunnelers.Shaders;
/// <summary>
/// Simple wrapper for a <see cref="ShaderMaterial" /> that uses a particular <see cref="Shader"/>;
/// as a more strict method of interaction with said <c>Shader</c>.
/// 
/// Implementations should publish methods for interacting with the aforementioned <c>Shader</c>'s parameters.
/// </summary>
/// <remarks>
/// Remember, as <c>ShaderMaterials</c> are <see cref="Resource">Resources</see>, that if every instance that utilizes the provided material should have a unique set of parameters,
/// it must either be made local to the scene, or a new <c>ShaderMaterial</c> should be created for each instance.
/// </remarks>
public abstract class ShaderInstance {
  /// <summary>
  /// An instance of a ShaderMaterial, which utilizes the gdshader that this implementation of ShaderInstance expects
  /// </summary>
  protected readonly ShaderMaterial ShaderMaterial;

  /// <summary>
  /// The specific <c>gdshader</c> that we are expecting this particular implementation to utilize.
  /// ShaderMaterials will have this validated at construction time - and will error out if it does not match, or is otherwise missing.
  /// Clients can optionally allow <c>setShader</c> at construction time, in which case the shader, if incorrect/not set,
  /// will be set to the correct one at runtime.
  /// </summary>
  /// <remarks>
  /// If an implementation of this is frequently used to <i>set</i> a shader, it should overload <see cref="GetShaderResource" /> and
  /// duplicate from an already loaded instance of the related Shader in question
  /// </remarks>
  protected abstract string ShaderResourcePath { get; }
  
  /// <param name="shaderMaterial">The <see cref="ShaderMaterial"/> that this instance manages the <see cref="Shader"/> of</param>
  /// <param name="setShader">Whether or not the shader should be set if it is incorrect or null</param>
  /// <exception cref="NoNullAllowedException"><c>shaderMaterial</c> cannot be null</exception>
  /// <exception cref="BadShaderException">Thrown if the provided <see cref="ShaderMaterial"/> is either missing or does not match the shader this class manages, <i>and</i> <c>setShader</c> is not set/false</exception>
  protected ShaderInstance(ShaderMaterial shaderMaterial, bool setShader = false) {
    ShaderMaterial = shaderMaterial ?? throw new NoNullAllowedException();
    // validate that the provided material has the expected shader
    if (IsExpectedShaderPath()) return;
    if (!setShader) throw new BadShaderException();
    SetShader();
  }

  private bool IsExpectedShaderPath() {
    return ShaderMaterial?.Shader is not null
           && ShaderMaterial.Shader.ResourcePath == ShaderResourcePath;
  }

  private void SetShader() => ShaderMaterial.SetShader(GetShaderResource());

  /// <summary>
  /// Gets a new instance of our <see cref="Shader"/>.
  /// This will be called internally by the base class when <c>setShader</c> is true.
  /// </summary>
  /// <remarks>
  /// Implementations utilizing this class as a way of <i>setting</i> a <c>Shader</c> should preload the <c>Shader</c>
  /// (like in a static var, or an instance on a Service Locator of sorts) and override this method to use or clone that <see cref="Resource"/> instead.
  /// </remarks>
  protected virtual Shader GetShaderResource() {
    var shd = GD.Load<Shader>(ShaderResourcePath);
    return shd;
  }

  private class BadShaderException() : System.Exception(
    "The provided ShaderMaterial either did not have a shader, or did not have a path that matched the expected one, and setShader was not enabled"
  );
}
