import { useEffect, useRef, useState } from "react";
import IApplication from "../../../interfaces/IApplication";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import IOrganization from "../../../interfaces/IOrganization";

import "./ApproveForm.css";
import CustomInput from "../../layout/CustomInput";

interface IApproveForm {
    application: IApplication
}

export const ApproveForm: React.FunctionComponent<IApproveForm> = (props) => {
    const { application } = props;

    const axiosPrivate = useAxiosPrivate();
    const onLoad = useRef(false);

    const [organizationStatus, setOrganizationStatus] = useState<boolean>(false);
    const [organizationInputs, setOrganizationInputs] = useState<{
        inn: string,
        trrc: string
    }>({
        inn: '',
        trrc: ''
    });

    const [isValid, setIsValid] = useState<boolean>(true);
    const [isExist, setIsExist] = useState<boolean>(false);
    const [organizationResult, setOrganizationResult] = useState<boolean>(false);

    const handleInputChange = (event: React.SyntheticEvent) => {
        const target = event.target;
        if (target instanceof HTMLInputElement) {
            setOrganizationInputs({
                ...organizationInputs,
                [target.name]: [target.value]
            })
        }
    }

    const handleApproveSubmit = (event: React.SyntheticEvent) => {
        event.preventDefault();

        setIsExist(false);
        setIsValid(true);

        const getOrganization = async () => {
            try {
                const response = await axiosPrivate.get(`api/organizations/${ organizationInputs.inn }/${ organizationInputs.trrc }`);
                setIsExist(response.data);
            } catch (error) {
                console.error(error);
            }
        }

        const approveOrganization = async () => {
            try {
                await axiosPrivate.patch(`api/organizations/${ application.organizationId }/approve`, {
                    inn: Number(organizationInputs.inn),
                    trrc: Number(organizationInputs.trrc)
                });
                setOrganizationResult(true);
            } catch (error) {
                console.error(error);
            }
        }

        const approveApplication = async () => {
            try {
                await axiosPrivate.patch(`api/applications/${ application.id }/approve`);
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        if (organizationStatus) {
            approveApplication();
        } else {
            if (organizationInputs.inn === '' || organizationInputs.trrc === '') {
                setIsValid(false);
            } else {
                getOrganization()
                    .then(async () => {
                        !isExist && approveOrganization()
                    });
            }
        }
    }

    useEffect(() => {
        const approveApplication = async () => {
            try {
                await axiosPrivate.patch(`api/applications/${ application.id }/approve`);
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }
        
        if (organizationResult) {
            approveApplication();
        }
    }, [organizationResult])

    useEffect(() => {
        const getOrganization = async () => {
            try {
                const response = await axiosPrivate.get(`api/organizations/${ application.organizationId }`);
                const organization: IOrganization = response.data;
                setOrganizationStatus(organization.isApproved);
            } catch (error) {
                console.error(error);
            }
        }

        onLoad.current && getOrganization();

        onLoad.current = true;
    }, []);

    return (
        <form className="approve-form" onSubmit={ handleApproveSubmit }>
            <div className="approve-form-title">
                <h3>Согласование</h3>
            </div>
            {
                organizationStatus ?
                <div className="approve-form-content">
                    <p>Вы уверены, что хотите согласовать заявку?</p>
                    <p className="warning">После согласования отменить действие будет нельзя</p>
                </div>
                :
                <div className="approve-form-content">
                    <p>
                        Чтобы согласовать заявку, необходимо ввести
                        <br />
                        дополнительные данные о компании
                    </p>
                    <div className={ `approve-inputs` }>
                        <CustomInput
                            collection={ `approve-org` }
                            label={ `ИНН` }
                            name={ `inn` }
                            value={ organizationInputs.inn }
                            onChange={ handleInputChange }
                        />
                        <CustomInput
                            collection={ `approve-org` }
                            label={ `КПП` }
                            name={ `trrc` }
                            value={ organizationInputs.trrc }
                            onChange={ handleInputChange }
                        />
                    </div>
                    { isExist && <p className="warning">Организация с такими данными уже существует</p> }
                    { !isValid && <p className="warning">Не все поля заполнены</p> }
                </div>
            }
            <div className="buttons-container">
                <button type="submit" className="button">Подтвердить</button>
            </div>
        </form>
    );
}