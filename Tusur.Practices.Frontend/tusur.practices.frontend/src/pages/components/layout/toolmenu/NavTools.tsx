function NavTools(props: { children: React.ReactNode }) {
    return (
        <ul className="nav-tools">
            { 
                Array.isArray(props.children)
                    ? props.children.map(child => <li className="nav-tools-element tool-menu-element">{ child }</li> )
                    : <li className="nav-tools-element tool-menu-element">{ props.children }</li>
            }
        </ul>
    );
}

export default NavTools;