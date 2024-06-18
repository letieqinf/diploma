function AccountTools(props: { children: React.ReactNode }) {
    return (
        <ul className="account-tools">
            {
                Array.isArray(props.children)
                    ? props.children.map(child => <li className="account-tools-element tool-menu-element">{ child }</li>)
                    : <li className="account-tools-element tool-menu-element">{ props.children }</li>
            }
        </ul>
    );
}

export default AccountTools;