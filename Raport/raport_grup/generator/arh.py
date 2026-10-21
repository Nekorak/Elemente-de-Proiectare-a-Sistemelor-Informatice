import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import FancyBboxPatch
G="#1e5b46"; Y="#e8a317"; L="#dde8e2"
fig, ax = plt.subplots(figsize=(16, 9), dpi=130); ax.set_xlim(0,16); ax.set_ylim(0,9); ax.axis("off")
def b(x,y,w,h,t,fc="white",ec=G,tc="#1b1b1b",fs=11,bold=False,lw=1.5):
    ax.add_patch(FancyBboxPatch((x,y),w,h,boxstyle="round,pad=0.03",fc=fc,ec=ec,lw=lw))
    ax.text(x+w/2,y+h/2,t,ha="center",va="center",fontsize=fs,color=tc,fontweight="bold" if bold else "normal",family="DejaVu Sans")
def arr(p,q,t="",c="#444",ls="-",off=(0,0.18)):
    ax.annotate("",xy=q,xytext=p,arrowprops=dict(arrowstyle="-|>",color=c,lw=1.6,ls=ls))
    if t: ax.text((p[0]+q[0])/2+off[0],(p[1]+q[1])/2+off[1],t,ha="center",fontsize=9.5,color=c,family="DejaVu Sans")
# client
ax.add_patch(FancyBboxPatch((0.2,0.3),6.4,8.3,boxstyle="round,pad=0.05",fc=L,ec=G,lw=1.2))
ax.text(3.4,8.3,"Stația de lucru (laptop) — client .NET 10",ha="center",fontsize=12,fontweight="bold",color=G,family="DejaVu Sans")
b(0.6,6.8,5.6,1.0,"Autogara.WinForms\nformulare, controale desenate, iconițe SVG",fc=G,tc="white",bold=True,fs=10.5)
b(0.6,5.2,5.6,1.0,"Autogara.Business — AutogaraBackend\nservicii, reguli, securitate, cache, actualizări",fs=10.5)
b(0.6,4.0,4.4,0.9,"Autogara.DataAccess\nEF Core + proceduri stocate",fs=10)
b(0.6,2.9,5.6,0.8,"Autogara.FileServer — client SFTP (SSH.NET), JSON + validare",fs=10)
b(0.6,1.7,2.7,0.9,"Autogara.Domain\nentități, enumerări",fs=10)
b(3.5,1.7,2.7,0.9,"Autogara.Common\nconfigurare DPAPI, log",fs=10)
b(0.6,0.55,5.6,0.8,"Local: appsettings.local.json, Logs, Cache (offline)",fs=9.5,ec="#888")
arr((3.4,6.8),(3.4,6.2)); arr((2.8,5.2),(2.8,4.9)); arr((5.6,5.2),(5.6,3.7))
# network
b(7.3,3.6,2.2,1.8,"Internet\n↓\nRouter (NAT)\n14330 → 14330\n44545 → 22",fc="#fff7e3",ec=Y,fs=10)
# server
ax.add_patch(FancyBboxPatch((10.1,0.3),5.7,8.3,boxstyle="round,pad=0.05",fc=L,ec=G,lw=1.2))
ax.text(12.95,8.3,"Serverul — computerul „PC” (192.168.1.15)",ha="center",fontsize=12,fontweight="bold",color=G,family="DejaVu Sans")
b(10.5,5.0,4.9,2.7,"MS SQL Server — instanța EPSI, TCP 14330\n\nbaza autogara (schema autogara)\n17 tabele · 5 viziuni · 6 funcții\n10 proceduri stocate · 15 indici\nSQL Server Agent: 6 joburi",fs=10)
b(10.5,1.0,4.9,3.3,"OpenSSH Server (sshd), port 22 — SFTP\n\nFile-Server/\n  Autobuze/autobuz_{id}.json\n  Harta/harta.json\n  Backup/{folder}/{dată}/\n  Rapoarte/ (CSV arhivate)",fs=10)
arr((5.0,4.45),(7.3,4.75),"SQL criptat",off=(0.6,0.12)); arr((9.5,5.0),(10.5,6.2))
arr((6.2,3.3),(7.3,4.0),"SFTP",off=(0.1,-0.45)); arr((9.5,4.1),(10.5,2.8))
plt.savefig("Arhitectura.png",bbox_inches="tight",facecolor="white")
