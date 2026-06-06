using Godot;
using System;

namespace Metarract.GameLib.Godot;
public class SimpleTimer(ulong timeoutCd, Action onTimeout) {
  private ulong _timeoutCd = timeoutCd;
  private readonly Action _onTimeout = onTimeout;
  public ulong StartupTimestamp { get; private set; }
  public ulong Timeout { get; private set; }
  public bool Done { get; private set; } = true;

  public SimpleTimer(ulong timeoutCd) : this(timeoutCd, () => { }) { }

  public void SetNewTimeout(ulong newCd) => _timeoutCd = newCd;
  
  public void Start() {
    StartupTimestamp = Time.GetTicksMsec();
    Timeout = Time.GetTicksMsec() + _timeoutCd;
    Done = false;
  }

  public bool Check() {
    if (Done) return true;
    if (Time.GetTicksMsec() < Timeout) return false;
    _onTimeout();
    Done = true;
    return true;
  }
}
