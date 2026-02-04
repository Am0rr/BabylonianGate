import { 
  FolderClock, 
  FolderCheck,
  Folders,
  FolderHeart,
  AlertCircle,
  Wrench, 
  Crosshair
} from 'lucide-react';

interface StatsBannerProps {
    onFilterClick?: (filter: string) => void;
}

export const StatsBanner = ({ onFilterClick }: StatsBannerProps) => {
  return (
    <div className="w-full bg-[#0A0A0A] border border-white/15 rounded-md overflow-hidden mb-6 shadow-lg">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 divide-y md:divide-y-0 md:divide-x divide-white/10">
        
        <div 
            onClick={() => onFilterClick?.('ready')}
            className="p-6 cursor-pointer hover:bg-white/[0.03] transition-colors relative group"
        >
            <div className="flex items-center gap-3 mb-4">
                <div className="p-2 bg-emerald-500/10 rounded-md text-emerald-500">
                    <FolderCheck size={20} />
                </div>
                <div>
                    <h3 className="text-sm font-bold text-white tracking-wider uppercase">Storage</h3>
                    <p className="text-xs text-gray-400">Ready for deployment</p>
                </div>
            </div>

            <div className="flex items-end gap-3">
                <span className="text-4xl font-bold text-white font-mono leading-none">154</span>
                <div className="mb-1 flex items-center gap-2">
                    <span className="text-sm font-bold text-emerald-500">84%</span>
                    <span className="text-xs text-gray-500 uppercase font-medium">of Total Arsenal</span>
                </div>
            </div>
            
            <div className="w-full bg-gray-800 h-1.5 mt-4 rounded-full overflow-hidden">
                <div className="bg-emerald-500 h-full w-[84%]"></div>
            </div>
        </div>

        <div className="p-6 relative hover:bg-white/[0.01] transition-colors">
             <div className="flex items-center gap-3 mb-4">
                <div className="p-2 bg-amber-500/10 rounded-md text-amber-500">
                    <FolderClock size={20} />
                </div>
                <div>
                    <h3 className="text-sm font-bold text-white tracking-wider uppercase">Operational Overview</h3>
                    <p className="text-xs text-gray-400">Asset distribution status</p>
                </div>
            </div>

            <div className="flex gap-6 mt-2 items-start">
                <div 
                    onClick={() => onFilterClick?.('deployed')}
                    className="group/stat cursor-pointer"
                >
                    <span className="text-4xl font-bold text-amber-500 font-mono leading-none drop-shadow-[0_0_8px_rgba(242, 121, 53)]">03</span>
                    <div className="text-[10px] uppercase font-bold text-amber-400 mt-1 flex items-center gap-1">
                         Deployed <Crosshair size={10} />
                    </div>
                </div>
            
                <div className="w-px bg-white/10 h-10 self-center"></div>

                <div 
                    onClick={() => onFilterClick?.('repair')}
                    className="group/stat cursor-pointer"
                >
                    <span className="text-4xl font-bold text-blue-500 font-mono leading-none drop-shadow-[0_0_8px_rgba(242, 121, 53)]">03</span>
                    <div className="text-[10px] uppercase font-bold text-blue-400 mt-1 flex items-center gap-1">
                         Repairing <Wrench size={10} />
                    </div>
                </div>

                <div className="w-px bg-white/10 h-10 self-center"></div>

                <div 
                    onClick={() => onFilterClick?.('missing')}
                    className="group/stat cursor-pointer relative"
                >
                    <span className="text-4xl font-bold text-rose-500 font-mono leading-none drop-shadow-[0_0_8px_rgba(244,63,94,0.3)]">03</span>
                    <div className="text-[10px] uppercase font-bold text-rose-400 mt-1 flex items-center gap-1">
                         Missing <AlertCircle size={10} />
                    </div>
                </div>
            </div>
        </div>

        <div 
            onClick={() => onFilterClick?.('ammos')}
            className="p-6 cursor-pointer hover:bg-white/[0.03] transition-colors relative"
        >
            <div className="flex justify-between items-start mb-4">
                <div className="flex items-center gap-3">
                    <div className="p-2 bg-blue-500/10 rounded-md text-blue-500"> 
                        <Folders size={20} />
                    </div>
                    <div>
                        <h3 className="text-sm font-bold text-white tracking-wider uppercase">Munitions</h3>
                        <p className="text-xs text-gray-400">Total Supply</p>
                    </div>
                </div>
                <div className="px-2 py-1 bg-blue-500/10 rounded text-[10px] font-bold text-blue-400 uppercase tracking-wide">
                    Optimal
                </div>
            </div>

            <div className="flex gap-4 mt-2 items-end">
                <div>
                    <span className="text-4xl font-bold text-white font-mono leading-none">42.3K</span>
                    <div className="text-[10px] uppercase font-bold text-gray-500 mt-1">Total Rounds</div>
                </div>
                <div className="h-8 w-px bg-white/10 mb-1"></div>
                <div className="mb-0.5">
                    <span className="text-xl font-bold text-white font-mono leading-none text-blue-200">15</span>
                    <div className="text-[10px] uppercase font-bold text-blue-400 mt-1">Crates</div>
                </div>
            </div>
        </div>

        <div 
             onClick={() => onFilterClick?.('personnel')}
             className="p-6 cursor-pointer hover:bg-white/[0.03] transition-colors relative"
        >
             <div className="flex justify-between items-start mb-4">
                <div className="flex items-center gap-3">
                    <div className="p-2 bg-violet-500/10 rounded-md text-violet-400">
                        <FolderHeart size={20} />
                    </div>
                    <div>
                        <h3 className="text-sm font-bold text-white tracking-wider uppercase">Personnel</h3>
                        <p className="text-xs text-gray-400">Armed Units</p>
                    </div>
                </div>
                <div className="px-2 py-1 bg-violet-500/10 rounded text-[10px] font-bold text-violet-400 uppercase tracking-wide">
                    On Duty
                </div>
            </div>

            <div className="flex items-end gap-3 mb-2">
                <span className="text-4xl font-bold text-white font-mono leading-none">34</span>
                <span className="text-xs text-gray-500 font-bold mb-1 uppercase">
                    / 120 Total Staff
                </span>
            </div>

            <div className="w-full flex items-center gap-2 mt-3">
                <div className="flex-1 bg-gray-800 h-1.5 rounded-full overflow-hidden">
                    <div className="bg-violet-500 h-full w-[28%]"></div>
                </div>
                <span className="text-[10px] font-mono text-violet-400">28%</span>
            </div>
        </div>
        
      </div>
    </div>
  );
};