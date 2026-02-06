import { 
  LayoutDashboard, 
  Box, 
  Users, 
  FileText, 
  Settings, 
  LogOut,
  ChevronRight,
  Database,
  Crosshair,
  type LucideIcon 
} from 'lucide-react';

interface MenuItem {
    id: string;
    icon: LucideIcon;
    label: string;
    alert?: boolean;   
    subtext?: string;   
    stat?: string;      
    badge?: number;     
}

interface MenuGroup {
    title: string;
    items: MenuItem[];
}

export const Sidebar = () => {
  const activeItem = "Armory"; 

  const menuGroups: MenuGroup[] = [
    {
      title: "OPERATIONAL", 
      items: [
        { id: "Dashboard", icon: LayoutDashboard, label: "Dashboard" },
        { 
          id: "Armory", 
          icon: Crosshair, 
          label: "Armory", 
          alert: true, 
          subtext: "Action Req." 
        }, 
        { 
            id: "Personnel", 
            icon: Users, 
            label: "Personnel",
            stat: "34 On Duty" 
        },
      ]
    },
    {
      title: "SYSTEM",
      items: [
        { id: "Logs", icon: FileText, label: "Access Logs", badge: 5 },
        { id: "Database", icon: Database, label: "Database" },
        { id: "Settings", icon: Settings, label: "Settings" },
      ]
    }
  ];

  return (
    <aside className="w-72 bg-[#050505] border-r border-white/10 flex flex-col h-screen font-mono select-none">
      
      <div className="h-16 flex items-center px-6 border-b border-white/5">
        <div className="w-10 h-10 bg-emerald-500/10 rounded flex items-center justify-center mr-3 border border-emerald-500/20 shadow-[0_0_10px_rgba(16,185,129,0.1)]">
          <Box className="text-emerald-500" size={25} />
        </div>
        <div>
            <h1 className="font-bold text-white tracking-wider text-[24px]">ARMORY WMS</h1>
            <p className="text-[10px] text-gray-600 font-medium tracking-widest uppercase">System Ver. 1.0</p>
        </div>
      </div>

      <div className="flex-1 overflow-y-auto py-6 space-y-8 scrollbar-hide">
        {menuGroups.map((group) => (
          <div key={group.title}>
            <h3 className="px-6 text-[20px] font-bold text-white uppercase tracking-[0.2em] mb-3">
              {group.title}
            </h3>
            
            <div className="space-y-1">
              {group.items.map((item) => {
                const isActive = activeItem === item.id;
                
                return (
                  <div 
                    key={item.id}
                    className={`
                      relative px-6 py-3 flex items-center cursor-pointer group transition-all duration-200
                      ${isActive ? 'bg-white/[0.04]' : 'hover:bg-white/[0.02]'}
                    `}
                  >
                    {isActive && (
                      <div className="absolute left-0 top-0 bottom-0 w-[3px] bg-emerald-500 shadow-[0_0_12px_rgba(16,185,129,0.6)]"></div>
                    )}

                    <div className={`mr-4 transition-colors ${isActive ? 'text-emerald-400' : 'text-gray-500 group-hover:text-gray-300'}`}>
                      <item.icon size={27} strokeWidth={1} />
                    </div>

                    <div className="flex-1">
                      <div className="flex justify-between items-center">
                        <span className={`text-[20px] font-medium  transition-colors ${isActive ? 'text-white' : 'text-gray-400 group-hover:text-white'}`}>
                          {item.label}
                        </span>

                        {item.badge !== undefined && (
                            <span className="bg-white/5 text-gray-300 text-[10px] px-2 py-0.5 rounded border border-white/10 font-mono">
                                {item.badge}
                            </span>
                        )}
                        
                        {item.alert && (
                            <div className="h-2 w-2 rounded-full bg-amber-500 animate-pulse shadow-[0_0_8px_rgba(245,158,11,0.6)]"></div>
                        )}
                      </div>
                      
                      {(item.stat || item.subtext) && (
                          <div className="text-xs mt-1 font-medium tracking-tight">
                              {item.stat && <span className="text-emerald-500/90 drop-shadow-sm">{item.stat}</span>}
                              {item.subtext && <span className="text-amber-500/90 drop-shadow-sm">{item.subtext}</span>}
                          </div>
                      )}
                    </div>

                    {!isActive && (
                        <ChevronRight size={14} className="text-gray-700 opacity-0 group-hover:opacity-100 transition-all -mr-2" />
                    )}
                  </div>
                );
              })}
            </div>
          </div>
        ))}
      </div>

      <div className="border-t border-white/5 p-4 bg-[#050505]">
        
        <div className="flex items-center gap-3 p-2 rounded-lg hover:bg-white/[0.03] transition-colors group cursor-default">
            
            <div className="w-10 h-10 rounded bg-[#0A0A0A] border border-white/10 overflow-hidden relative shadow-sm">
                <div className="absolute inset-0 flex items-center justify-center text-xs font-bold text-gray-500 bg-gradient-to-b from-transparent to-black/50">
                    CS
                </div>
                <div className="absolute bottom-0.5 right-0.5 w-2.5 h-2.5 bg-emerald-500 border-2 border-[#050505] rounded-full"></div>
            </div>

            <div className="flex-1 min-w-0">
                <h4 className="text-sm font-bold text-gray-200 group-hover:text-white transition-colors truncate">
                    CPL. SHEPARD
                </h4>
                <div className="text-[10px] text-gray-600 font-bold uppercase tracking-wider">
                    Commander
                </div>
            </div>

            <button className="text-gray-600 hover:text-white opacity-50 hover:opacity-100 transition-all active:scale-95 p-1">
                <LogOut size={18} strokeWidth={1.5} />
            </button>
        </div>
        
        <div className="mt-3 flex justify-between items-center px-2 text-[10px] text-zinc-600 font-mono tracking-tight">
            <span className="flex items-center gap-1.5">
                <span className="w-1.5 h-1.5 bg-emerald-900 rounded-full animate-pulse"></span>
                SERVER: ONLINE
            </span>
            <span>14:02 UTC</span>
        </div>
      </div>

    </aside>
  );
};