import { faClose } from '@fortawesome/free-solid-svg-icons';
import './ModalWindow.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function ModalWindow(props: { children: React.ReactNode, id: any, disabled: boolean, onClose: any }) {
    return (
        <>
            {
                props.disabled ? 
                <></>
                :
                <div className={"modal-window-container"}>
                    <div className="modal-window" id={props.id}>
                        <div className="content-container">
                            { props.children }
                        </div>
                        <FontAwesomeIcon icon={faClose} className="close-shortcut" onClick={() => props.onClose()} />
                    </div>
                </div>
            }
        </>
    );
}

ModalWindow.defaultProps = {
    children: undefined,
    id: undefined,
    disabled: false
};

export default ModalWindow;