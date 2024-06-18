import './ToolMenu.css'

function ToolMenu(props: { children: React.ReactNode }) {
    return (
        <header className="tool-menu">
            { props.children }
        </header>
    );
}

export default ToolMenu;