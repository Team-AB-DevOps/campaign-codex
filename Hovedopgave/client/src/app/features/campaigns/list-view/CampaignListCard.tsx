import ConfirmationDialog from '@/components/confirmation-dialog/ConfirmationDialog';
import { useAccount } from '@/lib/hooks/useAccount';
import { useCampaigns } from '@/lib/hooks/useCampaigns';
import { Campaign } from '@/lib/types';
import { Trash } from 'lucide-react';
import { useState } from 'react';
import { Link, useNavigate } from 'react-router';
import { toast } from 'react-toastify';

type Props = {
    campaign: Campaign;
};

export default function CampaignListCard({ campaign }: Props) {
    const [isDeleteCampaignDialogOpen, setIsDeleteCampaignDialogOpen] =
        useState(false);

    const { currentUser } = useAccount();
    const { deleteCampaign } = useCampaigns(campaign.id);

    const navigate = useNavigate();

    const isDM = currentUser?.id === campaign.dungeonMaster.id;

    const handleDeleteCampaign = () => {
        deleteCampaign.mutate(undefined, {
            onSuccess: () => {
                toast('Deleted campaign! 😎', {
                    type: 'success',
                });
                navigate('/campaigns');
            },
            onError: () => {
                toast('Something went wrong deleting campaign 😬', {
                    type: 'error',
                });
            },
        });
    };

    return (
        <>
            <Link
                to={`dashboard/${campaign.id}/map`}
                className='group relative rounded-lg border-2 border-yellow-500/30 bg-gradient-to-b from-stone-700 to-stone-800 p-5 transition-all hover:border-yellow-400/50 hover:shadow-md hover:shadow-yellow-500/10'
            >
                <div className='flex items-start justify-between'>
                    {isDM && (
                        <Trash
                            aria-label='Delete campaign'
                            className='absolute top-2 right-2 cursor-pointer rounded-full bg-stone-600 p-2 text-yellow-100 transition-all hover:text-red-400'
                            onClick={(e) => {
                                e.preventDefault();
                                setIsDeleteCampaignDialogOpen(true);
                            }}
                            size={35}
                        />
                    )}
                    <h3 className='font-cinzel text-xl font-bold text-yellow-100 group-hover:text-yellow-200'>
                        {campaign.name}
                    </h3>
                </div>
                <div className='mt-4 space-y-2 text-sm text-yellow-100/80'>
                    <div>
                        <span className='font-bold text-yellow-500'>DM:</span>{' '}
                        {campaign.dungeonMaster.displayName}
                    </div>
                    <div>
                        <span className='font-bold text-yellow-500'>
                            Players:
                        </span>{' '}
                        {campaign.players.length > 0 ? (
                            campaign.players
                                .map((p) => p.displayName)
                                .join(', ')
                        ) : (
                            <span className='opacity-30'>None</span>
                        )}
                    </div>
                </div>
            </Link>
            <ConfirmationDialog
                title='Delete Campaign?'
                description='This action cannot be undone, all campaign material will be lost!'
                isConfirmationDialogOpen={isDeleteCampaignDialogOpen}
                setIsConfirmationDialogOpen={setIsDeleteCampaignDialogOpen}
                handleConfirmation={handleDeleteCampaign}
            />
        </>
    );
}
