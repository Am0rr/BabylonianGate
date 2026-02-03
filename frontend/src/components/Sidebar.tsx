import { LayoutDashboard, Shield, Users, FileText, Settings } from 'lucide-react';

export const Sidebar = () => {
  const menuItems = [
    { icon: LayoutDashboard, label: 'Dashboard', active: false },
    { icon: Shield, label: 'Armory', active: true },
    { icon: Users, label: 'Personnel', active: false },
    { icon: FileText, label: 'Logs', active: false },
    { icon: Settings, label: 'Settings', active: false },
  ];

  return (
    <div className="w-80 h-screen bg-[#050505] border-r border-white/10 flex flex-col justify-between p-6 font-mono sticky top-0">
      
      <div>
        <div className="flex items-center justify-center pb-5 mb-8 border-b border-white/10">
          <h1 className="font-black text-3xl tracking-[0.2em] text-center leading-right">
            <span className="text-white block sm:inline">ARMORY</span>
            <span className="text-emerald-500 sm:ml-3 block sm:inline">WMS</span>
          </h1>
        </div>

        <nav className="space-y-2">
          {menuItems.map((item) => (
            <button
              key={item.label}
              className={`w-full flex items-center gap-4 px-4 py-3 text-sm tracking-wide transition-all duration-200 border-l-2 ${
                item.active 
                  ? 'bg-white/5 border-emerald-500 text-white font-bold shadow-[inset_10px_0_20px_-10px_rgba(16,185,129,0.1)]' 
                  : 'border-transparent text-gray-500 hover:text-gray-300 hover:bg-white/5'
              }`}
            >
              <item.icon size={22} className="opacity-80"/>
              <span className="text-xl font-medium pt-1">{item.label}</span>
            </button>
          ))}
        </nav>
      </div>

      <div className="pt-6 border-t border-white/10 space-y-4">
        
        <div className="bg-[#0F0F0F] border border-emerald-900/30 rounded p-3 flex items-center justify-between shadow-inner opacity-70 hover:opacity-100 transition-opacity cursor-help" title="Server Connection Status">
          <div className="flex items-center gap-3">
             <div className="relative flex h-2 w-2">
                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span>
                <span className="relative inline-flex rounded-full h-2 w-2 bg-emerald-500"></span>
              </div>
              <span className="text-xs font-bold text-emerald-500 tracking-widest">SYSTEM ONLINE</span>
          </div>
          <span className="text-[10px] text-emerald-900 font-mono">11282</span>
        </div>

        <div className="text-center">
            <span className="text-[9px] text-gray-700 uppercase tracking-[0.2em]">v1.0.3 Stable Build</span>
        </div>
      </div>

    </div>
  );
};