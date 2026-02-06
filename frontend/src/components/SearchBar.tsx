import { Search, X, SlidersHorizontal, Plus } from 'lucide-react';

interface SearchBarProps {
    value: string;
    onChange: (value: string) => void;
    placeholder?: string;
    onAddClick?: () => void;
}

export const SearchBar = ({
    value,
    onChange,
    placeholder = "Search by Serial Number...",
    onAddClick
}: SearchBarProps) => {

    return (
        <div className="w-full flex items-center justify-between gap-4 mb-6">
            
            <div className="flex-1 flex items-center gap-4 max-w-2xl">
                
                {/* Поле ввода */}
                <div className="relative flex-1 group">
                    <div className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-500 group-focus-within:text-emerald-500 transition-colors">
                        <Search size={20} />
                    </div>  

                    <input 
                        type="text"
                        value={value}
                        onChange={(e) => onChange(e.target.value)}
                        placeholder={placeholder}
                        className="w-full bg-[#0A0A0A] border border-white/15 rounded-md py-3 pl-12 pr-12 text-sm text-white placeholder:text-gray-600 focus:outline-none focus:border-emerald-500/50  focus:ring-1 focus:ring-emerald-500/50 transition-all font-mono"
                    />

                    {value.length > 0 && (
                        <button 
                            onClick={() => onChange('')}
                            className="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-white transition-colors"
                        >
                            <X size={16} />
                        </button>
                    )}
                </div>
                
                <button className="h-[46px] px-4 bg-[#0A0A0A] border border-white/15 rounded-md flex items-center gap-2 text-gray-400 hover:text-white hover:border-white/30 transition-all active:scale-95">
                    <SlidersHorizontal size={18} />
                    <span className="text-sm font-medium hidden sm:inline">Filters</span>
                </button>
            </div>

            <button 
                onClick={onAddClick}
                className="h-[46px] px-6 bg-emerald-600 hover:bg-emerald-500 border border-emerald-500/50 rounded-md flex items-center gap-2 text-white shadow-[0_0_15px_rgba(16,185,129,0.2)] hover:shadow-[0_0_25px_rgba(16,185,129,0.4)] transition-all active:scale-95 whitespace-nowrap"
            >
                <Plus size={18} />
                <span className="text-sm font-bold uppercase tracking-wide hidden sm:inline">Add New</span>
            </button>

        </div>
    );
};