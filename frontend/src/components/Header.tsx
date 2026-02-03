import { ChevronRight, Bell } from 'lucide-react';

export const Header = () => {
    return (
        <header className="flex h-20 items-center justify-between px-8 border-b border-white/10 bg-[#050505]">

            <div className="flex items-center gap-2 text-sm tracking-widest uppercase">
                <span className="text-gray-500 font-medium">Armory</span>
                <ChevronRight size={14} className="text-gray-600" />
                <span className="text-white font-bold">Weapons_DB</span>
            </div>

            <div className="flex items-center gap-6">

                <button className="relative text-gray-500 hover:text-white transition-colors">
                    <Bell size={20} />
                    <span className="absolute top-0 right-0 w-2 h-2 bg-emerald-500 rounded-full border-2 border-[#050505]"></span>
                </button>

                <div className="h-8 w-px bg-white/10"></div>

                <div className="flex items-center gap-3">
                    <div className="text-right hidden md:block">
                        <div className="text-sm font-bold text-white tracking-wide">CPL. SHEPARD</div>
                        <div className="text-[10px] text-emerald-500 uppercase tracking-wider font-mono">ID: 892-A</div>
                    </div>

                    <div className="h-10 w-10 rounded bg-emerald-900/20 border border-emerald-500/50 flex items-center justify-center overflow-hidden">
                        <img
                            src="https://api.dicebear.com/7.x/avataaars/svg?seed=Shepard&backgroundColor=b6e3f4"
                            alt="User"
                            className="h-full w-full opacity-80"
                            />
                    </div>
                </div>
            </div>
        </header>
    );
};